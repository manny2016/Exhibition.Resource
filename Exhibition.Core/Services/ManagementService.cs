


namespace Exhibition.Core.Services
{   
    using System.Collections.Generic;
    using System.IO;
    using Models = Exhibition.Core.Models;
    using System.Linq;
    using Exhibition.Core.Models;
    using Exhibition.Core;
    using Microsoft.AspNetCore.Http;

    public class ManagementService
    {
        #region FileSystem Management
        public IEnumerable<Resource> QueryResource(QueryFilter filter)
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

        public Resource CreateDirectory(string workspace, string directoryName)
        {
            var directory = new DirectoryInfo(Path.Combine(workspace.ServerMap(), directoryName))
                .CreateIfNotExists();
            return new Resource()
            {
                Workspace = workspace,
                Name = directoryName,
                FullName = directory.FullName.UrlMap(),
                Sorting = 0,
                Type = ResourceTypes.Folder
            };
        }

        public IEnumerable<Resource> DeleteResource(string workspace, string name)
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
                    yield return new Resource()
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

        public Resource Create(IFormFile file, string current)
        {
            var newfile = new FileInfo(string.Concat(current, "/", file.FileName).ServerMapFilePath());
            if (newfile.Exists) throw new FileUpoadException($"file name ({file.Name}) already exist. not allow to upload. please choose other name");

            newfile.Directory.CreateIfNotExists();

            using (var stream = new FileStream(newfile.FullName, FileMode.Create))
            {
                file.CopyToAsync(stream).GetAwaiter().GetResult();
            }
            return new Resource()
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
        public Resource Rename(string workspace, string name, string newly)
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
            return new Resource()
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
            return null;
        }
        public Models::Terminal DeleteTerminal(string ip)
        {
            return null;
        }
        public IEnumerable<Models::Terminal> QueryTerminals()
        {
            return null;
        }
        #endregion
        #region  Directive management

        #endregion

    }
}
