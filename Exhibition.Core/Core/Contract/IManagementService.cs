
namespace Exhibition.Core
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
    public interface IManagementService
    {

        #region FileSystem Management
        IEnumerable<Models::Resource> QueryResource(Models::QueryFilter filter);


        Models::Resource CreateDirectory(string workspace, string directoryName);


        IEnumerable<Models::Resource> DeleteResource(string workspace, string name);


        Models::Resource Create(IFormFile file, string current);

        /// <summary>
        /// Change file or folder name
        /// </summary>
        /// <param name="workspace">current directory</param>
        /// <param name="name">current file name</param>
        /// <param name="newly">new name</param>
        /// <returns></returns>
        Models::Resource Rename(string workspace, string name, string newly);


        IEnumerable<Models::Resource> QueryFileSystem(Models::QueryFilter filter);

        #endregion

        #region Terminal Management
        void CreateOrUpdate(IBaseTerminal terminal);

        void Delete(IBaseTerminal terminal);

        IEnumerable<IBaseTerminal> QueryTerminals(Models::SQLiteQueryFilter<string> filter);

        #endregion

        #region Directive Management
        void CreateOrUpdate(Models::Directive directive);

        void Delete(Models::Directive directive);

        IEnumerable<Models::Directive> QueryDirectives(Models::SQLiteQueryFilter<string> filter);
         void Run(IOperateContext context);
        #endregion
    }
}
