


namespace Exhibition.Core.Services
{
    using System.Collections.Generic;
    using System.IO;
    using Models = Exhibition.Core.Models;
    using Entities = Exhibition.Core.Entities;
    using System.Linq;

    using Exhibition.Core;
    using Microsoft.AspNetCore.Http;
    using System;
    using Dapper;
    public class ManagementService
    {
        #region FileSystem Management
        public IEnumerable<Models::Resource> QueryResource(Models::QueryFilter filter)
        {
            if (filter == null) yield break;
            var directory = new DirectoryInfo(filter.Current)
                .CreateIfNotExists();

            var files = string.IsNullOrEmpty(filter.Search)
                ? directory.GetFiles()
                : directory.GetFiles(filter.Search);
            foreach (var resource in files.Convert())
            {
                yield return resource;
            }
            foreach (var resource in directory.GetDirectories().Convert())
            {
                yield return resource;
            }

        }

        public Models::Resource CreateDirectory(string workspace, string directoryName)
        {
            var directory = new DirectoryInfo(Path.Combine(workspace.ServerMap(), directoryName))
                .CreateIfNotExists();
            return new Models::Resource()
            {
                Workspace = workspace,
                Name = directoryName,
                FullName = directory.FullName.UrlMap(),
                Sorting = 0,
                Type = ResourceTypes.Folder
            };
        }

        public IEnumerable<Models::Resource> DeleteResource(string workspace, string name)
        {
            var type = Path.Combine(workspace.ServerMap(), name).GetResourceType();
            switch (type)
            {
                case ResourceTypes.Folder:
                    var directory = new DirectoryInfo(Path.Combine(workspace.ServerMap(), name));
                    if (directory.Exists)
                    {
                        foreach (var resource in directory.GetFiles().Convert())
                        {
                            yield return resource;
                        }
                        directory.Delete(true);
                        yield break;
                    };
                    break;
                case ResourceTypes.H5:
                case ResourceTypes.Image:
                case ResourceTypes.Video:
                    var fileinfo = new FileInfo(Path.Combine(workspace.ServerMap(), name));
                    if (fileinfo.Exists)
                    {
                        fileinfo.Delete();
                    }
                    yield return new Models::Resource()
                    {
                        Workspace = workspace,
                        Name = name,
                        FullName = Path.Combine(workspace.ServerMap(), name).UrlMap(),
                        Type = type
                    };
                    break;
                default:
                    break;
            }



        }

        public Models::Resource Create(IFormFile file, string current)
        {
            var newfile = new FileInfo(string.Concat(current, "/", file.FileName).ServerMapFilePath());
            if (newfile.Exists) throw new FileUpoadException($"file name ({file.Name}) already exist. not allow to upload. please choose other name");

            newfile.Directory.CreateIfNotExists();

            using (var stream = new FileStream(newfile.FullName, FileMode.Create))
            {
                file.CopyToAsync(stream).GetAwaiter().GetResult();
            }
            return new Models::Resource()
            {
                Workspace = current,
                FullName = newfile.FullName.UrlMap(),
                Name = newfile.Name,
                Type = newfile.FullName.GetResourceType()
            };
        }
        /// <summary>
        /// Change file or folder name
        /// </summary>
        /// <param name="workspace">current directory</param>
        /// <param name="name">current file name</param>
        /// <param name="newly">new name</param>
        /// <returns></returns>
        public Models::Resource Rename(string workspace, string name, string newly)
        {
            var current = Path.Combine(workspace.ServerMap(), name);
            var type = current.GetResourceType();

            switch (type)
            {
                case ResourceTypes.Folder:
                    var directory = new DirectoryInfo(current);
                    if (!directory.Exists)
                    {
                        throw new DirectoryNotFoundException();
                    }
                    else
                    {
                        directory.MoveTo(Path.Combine(directory.Parent.FullName, newly));
                    }
                    break;
                case ResourceTypes.Image:
                case ResourceTypes.Video:
                case ResourceTypes.SerialPortDirective:
                case ResourceTypes.H5:
                default:
                    var fileinfo = new FileInfo(current);
                    if (!fileinfo.Exists)
                    {
                        throw new FileNotFoundException();
                    }
                    else
                    {
                        fileinfo.MoveTo(Path.Combine(workspace.ServerMap(), newly));
                    }
                    break;
            }
            return new Models::Resource()
            {
                Workspace = workspace,
                FullName = Path.Combine(current, newly).UrlMap(),
                Name = newly,
                Sorting = 0,
                Type = type
            };
        }

        public IEnumerable<Models::Resource> QueryFileSystem(Models::QueryFilter filter)
        {
            var root = filter.Current.ServerMap();
            var directory = new DirectoryInfo(root);
            foreach (var resource in FindSubDirectory(directory, filter.Search))
            {
                yield return resource;
            }
        }
        private IEnumerable<Models::Resource> FindSubDirectory(DirectoryInfo directory, string search)
        {
            foreach (var info in directory.GetFiles())
            {
                if (string.IsNullOrEmpty(search) == false && info.Name.IndexOf(search) < 0)
                    continue;
                yield return info.Convert();
            }
            yield return directory.Convert();

            foreach (var subdir in directory.GetDirectories())
            {
                foreach (var resource in FindSubDirectory(subdir, search))
                {
                    yield return resource;
                }
            }
        }
        #endregion

        #region  Terminal management
        public Models::Terminal CreateOrUpdate(Models::Terminal terminal)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = string.Empty;
                if (database.ExecuteScalar<int>("SELECT COUNT(*) FROM terminal WHERE Ip=@ip",
                    terminal.GenernateParameters()) > 0)
                {
                    database.Execute(terminal.GenernateUpdateScript(), terminal.GenernateParameters());
                }
                else
                {
                    database.Execute(terminal.GenernateInsertScript(), terminal.GenernateParameters());
                }
            }
            return terminal;
        }
        public Models::Terminal DeleteTerminal(Models::Terminal terminal)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                database.Execute(terminal.GenernateDeleteScript(), terminal.GenernateParameters());
            }
            return terminal;
        }
        public Models::Terminal QueryTerminal(string ip)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = "SELECT * FROM Terminal WHERE Ip=@ip";
                return database.QueryFirst<Entities::Terminal>(queryString, new { @ip = ip })
                    .Convert();
            }
        }
        public IEnumerable<Models::Terminal> QueryTerminals(string search = null)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = "SELECT * FROM Terminal  ";
                if (string.IsNullOrEmpty(search))
                {

                }
                return database.Query<Entities::Terminal>(queryString, new { @search = search })
                    .Select((ctx) =>
                    {
                        return new Models.Terminal()
                        {
                            Description = ctx.Description,
                            Endpoint = ctx.Endpoint,
                            Ip = ctx.Ip,
                            Name = ctx.Name,
                            Schematic = ctx.Schematic,
                            Windows = ctx.Windows.DeserializeToObject<Models::Window[]>()
                        };
                    });
            }
        }
        public int DeleteTerminal(string ip)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = @"
DELETE FROM Directive WHERE TargetIp =@ip;
DELETE FROM Terminal WHERE Ip=@ip";
                return database.Execute(queryString, new { @ip = ip });
            }
        }
        #endregion

        #region  Directive management
        public int CreateOrUpdate(Models::Directive directive)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = "SELECT COUNT(*) FROM Directive WHERE Name=@name";
                if (database.ExecuteScalar<int>(queryString, directive.GenernateParameters()) > 0)
                {
                    return database.Execute(directive.GenernateUpdateScript(), directive.GenernateParameters());
                }
                else
                {
                    return database.Execute(directive.GenernateInsertScript(), directive.GenernateParameters());
                }
            }
        }

        public int DeleteDirective(Models::Directive directive)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                return database.Execute(directive.GenernateDeleteScript(), directive.GenernateParameters());
            }
        }


        public IEnumerable<Models::Directive> QueryDirectives(Models::SQLiteDimQueryFilter filter = null)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var strWhere = " WHERE 1=1 ";
                var queryString = "SELECT * FROM Directive  ";
                if (filter != null && !string.IsNullOrEmpty(filter.Search))
                    strWhere = string.Concat(strWhere, " AND (NAME LIKE @search OR TargetIp LIKE @search)");

                queryString = string.Concat(queryString, strWhere);
                var results = database.Query<Entities::Directive>(queryString, new { @search = filter.Search });
                if (results == null) return new List<Models::Directive>();

                var ip = string.Join(",", results.Select((ctx) => { return $"'{ctx.TargetIp}'"; }));
                queryString = $@"SELECT * FROM Terminal WHERE Ip IN ({ip})";

                var terminals = database.Query<Entities.Terminal>(queryString);

                return results.Select((ctx) =>
                {
                    return ctx.Convert(terminals);
                });
            }
        }

        public Models::Directive QueryDirective(string name)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = "SELECT * FROM Directive WHERE Name = @name";
                var entity = database.QueryFirst<Entities::Directive>(queryString, new { @name = name });
                if (entity == null) return null;
                queryString = "SELECT * FROM Terminal WHERE Ip=@ip";
                var terminals = database.Query<Entities::Terminal>(queryString, new { @ip = entity.TargetIp });
                return entity.Convert(terminals);
            }
        }
        #endregion

    }
}
