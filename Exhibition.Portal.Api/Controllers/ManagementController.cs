


namespace Exhibition.Portal.Api.Controllers
{
    using Exhibition.Core;
    using Exhibition.Core.Models;
    using Exhibition.Core.Services;
    using Exhibition.Portal.Api.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models = Exhibition.Core.Models;

    [ApiController]
    public class ManagementController : ControllerBase
    {
        public ManagementService service = new ManagementService();

        [Route("api/mgr/GetFileSystem"), HttpPost]
        public QueryFileSystemResponse GetFileSystem(QueryFilter filter)
        {
            filter.Current = filter.Current ?? EnvironmentVariables.UrlROOT;
            filter.Current = filter.Current.TrimStart('/').TrimEnd('/');
            filter.Current = filter.Current.ServerMap();

            var resource = service.QueryResource(filter);
            return new QueryFileSystemResponse(filter.Current.UrlMap())
            {
                Data = service.QueryResource(filter).OrderBy(o => o.Type).ToArray(),
                Parent = filter.Current.ServerMap().GetParentWrokspace()
            };
        }

        [Route("api/mgr/CreateDirectory"), HttpPost]
        public ResourceActionResponse CreateDirectory(ResourceRequestContext context)
        {
            return new ResourceActionResponse()
            {
                Data = service.CreateDirectory(context.Workspace, context.Name)
            };
        }

        [Route("api/mgr/DeleteResource"), HttpPost]
        public QueryFileSystemResponse DeleteResource(ResourceRequestContext context)
        {
            return new QueryFileSystemResponse()
            {
                Data = service.DeleteResource(context.Workspace, context.Name).ToArray()
            };
        }

        [Route("api/mgr/UploadFiles"), HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public ResourceActionResponse[] UploadFiles(string workspace)
        {
            //var file = this.Request.Form.Files[0] as IFormFile;
            workspace = (workspace == null || workspace == "undefined") ? null : workspace;
            try
            {
                var list = new List<ResourceActionResponse>();
                foreach (var file in this.Request.Form.Files)
                {
                    var result = service.Create(file, workspace);
                    list.Add(new ResourceActionResponse() { Data = result });
                }
                return list.ToArray();
            }
            catch (FileUpoadException ex)
            {
                return new ResourceActionResponse[] {
                    new ResourceActionResponse(){
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResourceActionResponse[] {
                    new ResourceActionResponse(){
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                    }
                };
            }
        }

        [Route("api/mgr/Rename"), HttpPost]
        public ResourceActionResponse Rename(ResourceRequestContext context)
        {
            return new ResourceActionResponse()
            {
                Data = service.Rename(context.Workspace, context.Name, context.NewName)
            };
        }


        [Route("api/mgr/QueryTerminals"), HttpPost]
        public QueryTerminalResponse QueryTerminals(SQLiteDimQueryFilter filter)
        {
            return new QueryTerminalResponse()
            {
                Data = service.QueryTerminals(filter.Search).ToArray()
            };
        }

        [Route("api/mgr/CreateTerminal"), HttpPost]
        public TerminalActionResponse CreateTerminal(Models::Terminal terminal)
        {
            return new TerminalActionResponse()
            {
                Data = service.CreateOrUpdate(terminal)
            };
        }
        [Route("api/mgr/DeleteTerminal"), HttpPost]
        public GeneralResponse DeleteTerminal(Models::Terminal terminal)
        {
            if (terminal == null || string.IsNullOrEmpty(terminal.Ip)) return null;
            return new GeneralResponse()
            {
                Data = service.DeleteTerminal(terminal.Ip)
            };
        }

        [Route("api/mgr/QueryDirectives"), HttpPost]
        public QueryDirectiveResponse QueryDirectives(Models::SQLiteDimQueryFilter filter)
        {
            return new QueryDirectiveResponse()
            {
                Data = this.service.QueryDirectives(filter).ToArray()
            };
        }

        [Route("api/mgr/CreateOrUpdateDirective"), HttpPost]
        public GeneralResponse CreateOrUpdateDirective(Models::Directive directive)
        {
            return new GeneralResponse()
            {
                Data = this.service.CreateOrUpdate(directive)
            };
        }

        [Route("api/mgr/DeleteDirective"), HttpPost]
        public GeneralResponse DeleteDirective(Models::Directive directive)
        {
            return new GeneralResponse()
            {
                Data = this.service.DeleteDirective(directive)
            };
        }

        [Route("api/mgr/Execute"), HttpGet]
        public GeneralResponse Execute(string directiveName)
        {
            return new GeneralResponse() { Data = 0 };
        }
        [Route("api/mgr/QueryFileSystem"), HttpPost]
        public QueryFileSystemResponse QueryFileSystem(QueryFilter filter)
        {
            return new QueryFileSystemResponse()
            {
                Data = this.service.QueryFileSystem(filter).ToArray()
        };
    }
}
}