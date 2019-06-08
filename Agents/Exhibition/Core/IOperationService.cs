



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
        Models::GeneralResponse<int> Run(Models::OperationContext context);


        [OperationContract]
        [WebInvoke(Method = "GET",
         UriTemplate = "Readme",
         BodyStyle = WebMessageBodyStyle.Bare,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        string Readme();


        [OperationContract]
        [WebInvoke(Method = "GET",
         UriTemplate = "Shutdown",
         BodyStyle = WebMessageBodyStyle.Bare,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json)]
        void Shutdown();
    }
}
