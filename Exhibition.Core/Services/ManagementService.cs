


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
                case ResourceTypes.H5:
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
                default:
                    break;
            }
            return new Models::Resource()
            {
                Workspace = workspace,
                FullName = Path.Combine(workspace, newly).UrlMap(),
                Name = newly,
                Sorting = 0,
                Type = type
            };
        }


        #endregion

        #region  Terminal management
        public Models::Terminal CreateOrUpdateTerminal(Models::Terminal terminal)
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
        public IEnumerable<Models::Terminal> QueryTerminals(Models::SQLiteQueryFilter filter)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = string.Concat("SELECT * FROM terminal ", filter.GenernateWhereCase());
                var entities = database.Query<Entities::Terminal>(queryString);
                if (entities == null) return new List<Models::Terminal>();
                return entities.Select((ctx) =>
                {
                    return ctx.Convert();
                });
            }
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
        #endregion

        #region  Directive management
        public Models::Directive CreateOrUpdate(Models::Directive directive)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = "SELECT COUNT(*) FROM Directive WHERE Name=@name";
                if (database.ExecuteScalar<int>(queryString, directive.GenernateParameters()) > 0)
                {
                    database.Execute(directive.GenernateUpdateScript(), directive.GenernateParameters());
                }
                else
                {
                    database.Execute(directive.GenernateInsertScript(), directive.GenernateParameters());
                }
            }
            return directive;
        }

        public Models::Directive DeleteDirective(Models::Directive directive)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                database.Execute(directive.GenernateDeleteScript(), directive.GenernateParameters());
            }
            return directive;
        }
        public IEnumerable<Models::Directive> QueryDirectives(Models::SQLiteQueryFilter filter = null)
        {
            using (var database = SQLiteFactory.Genernate())
            {
                var queryString = string.Concat("SELECT * FROM Directive ", filter.GenernateWhereCase());
                var results = database.Query<Entities::Directive>(queryString);
                if (results == null) return new List<Models::Directive>();
                return results.Select((ctx) =>
                {
                    return ctx.Convert();
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
                return entity.Convert();
            }
        }
        #endregion

    }
}
