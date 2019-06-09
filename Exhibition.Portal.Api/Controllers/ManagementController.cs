


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
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ManagementController));
        [Route("api/mgr/GetFileSystem"), HttpPost, HttpOptions]
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

        [Route("api/mgr/QueryFileSystem"), HttpPost, HttpOptions]
        public GeneralResponse<Resource[]> QueryFileSystem(QueryFilter filter)
        {
            return new GeneralResponse<Resource[]>()
            {
                Data = this.service.QueryFileSystem(filter).ToArray()
            };
        }
        [Route("api/mgr/QueryFileSystemforChoosing"), HttpPost, HttpOptions]
        public GeneralResponse<SelectOptionGroup<Resource>[]> QueryFileSystemforChoosing(QueryFilter filter)
        {
            var selector = (filter.OnlyShowFolder ?? false)
                ? new Func<Resource, bool>(o => o.Type == ResourceTypes.Folder)
                : new Func<Resource, bool>(o => o.Type != ResourceTypes.Folder);
            return new GeneralResponse<SelectOptionGroup<Resource>[]>
            {
                Data = this.QueryFileSystem(filter).Data?
                .Where(selector)
                .GroupBy(o => o.Workspace)
                .Select((ctx) =>
                {
                    return new SelectOptionGroup<Resource>()
                    {
                        Items = ctx.Select((item) =>
                        {
                            return item;

                        }).ToArray(),
                        Name = ctx.Key
                    };
                }).Where(o => o.Name != "").ToArray()
            };
        }
        [Route("api/mgr/CreateDirectory"), HttpPost, HttpOptions]
        public GeneralResponse<Resource> CreateDirectory(ResourceRequestContext context)
        {
            return new GeneralResponse<Resource>()
            {
                Data = service.CreateDirectory(context.Workspace, context.Name)
            };
        }

        [Route("api/mgr/DeleteResource"), HttpPost, HttpOptions]
        public QueryFileSystemResponse DeleteResource(ResourceRequestContext context)
        {
            return new QueryFileSystemResponse()
            {
                Data = service.DeleteResource(context.Workspace, context.Name).ToArray()
            };
        }

        [Route("api/mgr/UploadFiles"), HttpPost, HttpOptions]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public GeneralResponse<Resource>[] UploadFiles(string workspace)
        {
            //var file = this.Request.Form.Files[0] as IFormFile;
            workspace = (workspace == null || workspace == "undefined") ? null : workspace;
            try
            {
                var list = new List<GeneralResponse<Resource>>();
                foreach (var file in this.Request.Form.Files)
                {
                    var result = service.Create(file, workspace);
                    list.Add(new GeneralResponse<Resource>() { Data = result });
                }
                return list.ToArray();
            }
            catch (FileUpoadException ex)
            {
                return new GeneralResponse<Resource>[] {
                    new GeneralResponse<Resource>(){
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                    }
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<Resource>[] {
                    new GeneralResponse<Resource>(){
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                    }
                };
            }
        }

        [Route("api/mgr/Rename"), HttpPost, HttpOptions]
        public GeneralResponse<Resource> Rename(ResourceRequestContext context)
        {
            return new GeneralResponse<Resource>()
            {
                Data = service.Rename(context.Workspace, context.Name, context.NewName)
            };
        }


        [Route("api/mgr/QueryTerminals"), HttpPost, HttpOptions]
        public GeneralResponse<IBaseTerminal[]> QueryTerminals(SQLiteQueryFilter<string> filter)
        {
            return new GeneralResponse<IBaseTerminal[]>()
            {
                Data = service.QueryTerminals(filter).ToArray()
            };
        }
        [Route("api/mgr/CreateOrUpdateSerialPortTerminal"), HttpPost, HttpOptions]
        public GeneralResponse<int> CreateOrUpdateSerialPortTerminal(SerialPortTerminal terminal)
        {
            service.CreateOrUpdate(terminal);
            return new GeneralResponse<int>();
        }

        [Route("api/mgr/CreateOrUpdateMediaPlayerTerminal"), HttpPost, HttpOptions]
        public GeneralResponse<int> CreateOrUpdateMediaPlayerTerminal(MediaPlayerTerminal terminal)
        {
            service.CreateOrUpdate(terminal);
            return new GeneralResponse<int>();
        }

        [Route("api/mgr/CreateOrUpdateDirective"), HttpPost, HttpOptions]
        public GeneralResponse<int> CreateOrUpdateDirective(DirectiveEditModel model)
        {
            var directive = new Models::Directive()
            {
                Name = model.Name,
                Resources = model.Resources,
                Description = model.Description
            };
            switch ((TerminalTypes)model.Terminal.type)
            {
                case TerminalTypes.MediaPlayer:
                    directive.Terminal = ((string)(model.Terminal.ToString())).DeserializeToObject<MediaPlayerTerminal>();
                    directive.DefaultWindow = model.DefaultWindow;
                    break;
                case TerminalTypes.SerialPort:
                    directive.Terminal = ((string)(model.Terminal.ToString())).DeserializeToObject<SerialPortTerminal>();
                    directive.DefaultWindow = null;
                    break;
            }
            service.CreateOrUpdate(directive);
            return new GeneralResponse<int>();
        }
        [Route("api/mgr/QueryDirectives"), HttpPost, HttpOptions]
        public GeneralResponse<Models::Directive[]> QueryDirectives(SQLiteQueryFilter<string> filter)
        {
            return new GeneralResponse<Models::Directive[]>()
            {
                Data = service.QueryDirectives(filter).ToArray()
            };
        }

        [Route("api/mgr/Execute"), HttpPost, HttpOptions]
        public GeneralResponse<int> Execute(OperateContext context)
        {
            Logger.Info($"Run directive:{context.SerializeToJson()}");
            var directive = this.service.QueryDirectives(new SQLiteQueryFilter<string>()
            {
                Keys = new string[] { context.Name },
                PrimaryKey = "Name",
            }).FirstOrDefault();
            var more = directive.Resources.Where(o => o.Type == ResourceTypes.Folder)
                .SelectMany((ctx) =>
                {
                    return service.QueryFileSystem(new QueryFilter() { Current = ctx.FullName });
                });
            if (more != null)
            {
                directive.Resources = directive.Resources.Concat(more).ToArray()
                    .Distinct(ResourceEqualityComparer.OrdinalIgnoreCase)
                    .Where(o=>o.Type!= ResourceTypes.Folder)
                    .ToArray();
            }
            this.service.Run(new OperationContext() { Type = context.Type, Directive = directive });
            return new GeneralResponse<int>();
        }
        [Route("api/mgr/Readme"), HttpGet]
        public string Readme()
        {
            return "I am a REST Api";
        }
    }
}