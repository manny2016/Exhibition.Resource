﻿
namespace Exhibition.Core
{

    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Exhibition.Core.Models;

    public static class FileSystemExtension
    {
        public static string ServerMap(this string relative)
        {
            if (string.IsNullOrEmpty(relative)) relative = EnvironmentVariables.ROOT.UrlMap();
            if (relative.StartsWith(EnvironmentVariables.UrlROOT) == false) relative = string.Concat(EnvironmentVariables.UrlROOT, "/", relative);
            var absolutely = Path.Combine(System.Environment.CurrentDirectory, relative.Replace("/", "\\"));
            if (Directory.Exists(absolutely))
                Directory.CreateDirectory(absolutely);
            return absolutely;
        }
        public static DirectoryInfo CreateIfNotExists(this DirectoryInfo directory)
        {
            if (!directory.Exists) directory.Create();
            return directory;
        }
        public static string ServerMapFilePath(this string relative)
        {
            var root = Path.Combine(System.Environment.CurrentDirectory, EnvironmentVariables.ROOT);
            return Path.Combine(root, relative.TrimStart('/').Replace("/", "\\"));
        }
        public static string UrlMap(this string absolutely)
        {
            var root = Path.Combine(System.Environment.CurrentDirectory, EnvironmentVariables.ROOT);
            return absolutely.Replace(root, string.Empty).Replace("\\", "/").TrimStart('/');
        }
        public static ResourceTypes GetResourceType(this string absolutely)
        {
            var directory = new DirectoryInfo(absolutely);
            var info = new FileInfo(absolutely);
            if (directory.Exists) return ResourceTypes.Folder;
            if (!info.Exists) return ResourceTypes.NotSupported;

            if (EnvironmentVariables.SupportImages.Any(o => o.Equals(info.Extension.ToLower()))) return ResourceTypes.Image;
            if (EnvironmentVariables.SupportVideos.Any(o => o.Equals(info.Extension.ToLower()))) return ResourceTypes.Video;
            if (EnvironmentVariables.SupportTxtPlain.Any(o => o.Equals(info.Extension.ToLower()))) return ResourceTypes.TextPlain;
            //if (EnvironmentVariables.SupportSerialPortDirective.Any(o => o.Equals(info.Extension.ToLower()))) return ResourceTypes.SerialPortDirective;
            return ResourceTypes.NotSupported;
        }
        public static IEnumerable<Resource> Convert(this FileInfo[] fileInfos)
        {
            if (fileInfos == null) yield break;
            foreach (var resource in fileInfos.Select(ctx => ctx.Convert())) {
                yield return resource;
                     
            };            
        }
        public static IEnumerable<Resource> Convert(this DirectoryInfo[] directories)
        {
            if (directories == null) yield break;
            foreach(var folder in directories.Select(ctx => ctx.Convert()))
            {
                yield return folder;
            }            
        }

        public static Resource Convert(this DirectoryInfo info)
        {
            return new Resource()
            {
                Workspace = info.FullName.UrlMap(),
                FullName =info.FullName.UrlMap(),
                Name = info.Name,
                Type = ResourceTypes.Folder,
                Sorting = 0

            };
        }
        public static Resource Convert(this FileInfo info)
        {
            return new Resource()
            {
                Name = info.Name,
                Type = info.FullName.GetResourceType(),
                FullName = info.FullName.UrlMap(),
                Workspace = info.Directory.FullName.UrlMap(),
                Sorting = 0,
            };
        }
        public static string GetParentWrokspace(this string absolutely)
        {
            var result = absolutely.UrlMap();
            var directory = new DirectoryInfo(absolutely);
            return string.IsNullOrEmpty(result)
                ? null
                : directory.Parent.FullName.UrlMap();

        }
    }
}
