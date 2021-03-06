﻿



namespace Exhibition.Core
{
    using Exhibition.Core.Models;
    using Newtonsoft.Json.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Models = Exhibition.Core.Models;
    using ShowModels = Exhibition.Agent.Show.Models;

    [ServiceContract]
    public interface IOperationService
    {


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Run",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Models::GeneralResponse<int> Run(ShowModels::OperationContext context);


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

        [OperationContract]
        [WebInvoke(Method = "POST",
        UriTemplate = "ShowLayoutInfo",
        BodyStyle = WebMessageBodyStyle.Bare,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
        void ShowLayoutInfo(Window[] windows);

        [OperationContract]
        [WebInvoke(Method = "POST",
      UriTemplate = "UpgradeLayout",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json)]
        void UpgradeLayout(Window[] windows);

    }
}
