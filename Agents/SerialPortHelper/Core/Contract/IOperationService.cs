

namespace SerialPortHelper
{
    using SerialPortHelper.Models;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using OperationContext = Models.OperationContext;

    [ServiceContract]
    public interface IOperationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
        UriTemplate = "Run",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        GeneralResponse<int> Run(OperationContext context);
    }
}
