﻿using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WCFLib
{
    [ServiceContract]
    public interface IWCF
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/static/{filename}",  RequestFormat = WebMessageFormat.Json,  ResponseFormat = WebMessageFormat.Json,  BodyStyle = WebMessageBodyStyle.Bare)]
        Stream ServeFile(string filename);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/app", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        Stream AppFile();
    }
}
