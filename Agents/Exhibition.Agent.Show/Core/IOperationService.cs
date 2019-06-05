



namespace Exhibition.Core
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Models = Exhibition.Core.Models;
    [ServiceContract]
    public interface IOperationService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Run",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Models::GeneralResponse<int> Run(Models::Directive directive);

    }
}
