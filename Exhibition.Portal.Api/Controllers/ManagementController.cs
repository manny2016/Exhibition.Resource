


namespace Exhibition.Portal.Api.Controllers
{
    using Exhibition.Core;
    using Exhibition.Core.Models;
    using Exhibition.Core.Services;
    using Exhibition.Portal.Api.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    public class ManagementController : ControllerBase
    {
        public ManagementService service = new ManagementService();

        [Route("api/msr/GetFileSystem"), HttpPost]
        public QueryFileSystemResponse GetFileSystem(QueryFilter filter)
        {
            filter.Current = filter.Current ?? EnvironmentVariables.UrlROOT;
            filter.Current = filter.Current.TrimStart('/').TrimEnd('/');
            filter.Current = filter.Current.ServerMap();

            var resource = service.QueryResource(filter);
            return new QueryFileSystemResponse(filter.Current.UrlMap())
            {
                Data = service.QueryResource(filter).OrderBy(o => o.Type).ToArray()
            };
        }


        [Route("api/msr/CreateDirectory"), HttpPost]
        public ResourceActionResponse CreateDirectory(ResourceRequestContext context)
        {            
            return new ResourceActionResponse()
            {
                Data = service.CreateDirectory(context.Workspace, context.Name)
            };
        }

        [Route("api/msr/DeleteResource"), HttpPost]
        public QueryFileSystemResponse DeleteResource(ResourceRequestContext context)
        {
            return new QueryFileSystemResponse()
            {
                Data = service.DeleteResource(context.Workspace, context.Name).ToArray()
            };
        }


        [Route("api/msr/UploadFiles"), HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public ResourceActionResponse UploadFiles(IFormFile file, string directory)
        {
            try
            {
                var result = service.Create(file, directory);
                return new ResourceActionResponse() { Data = result };
            }
            catch (FileUpoadException ex)
            {
                return new ResourceActionResponse()
                {
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                };
            }
            catch (Exception ex)
            {
                return new ResourceActionResponse()
                {
                    ErrorCode = 1001,
                    ErrorMsg = ex.Message,
                    Success = false
                };
            }

        }

        [Route("api/msr/Rename"), HttpPost]
        public ResourceActionResponse Rename(ResourceRequestContext context)
        {
            return new ResourceActionResponse()
            {
                Data = service.Rename(context.Workspace, context.Name, context.NewName)
            };
        }
    }
}