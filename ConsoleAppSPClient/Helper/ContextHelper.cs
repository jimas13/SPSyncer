using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSPClient.Helper;
public class ContextHelper
{
    public static ClientContext GetClientContext(string targetUrl, string accessToken)
    {
        ClientContext clientContext = new ClientContext(targetUrl);
        clientContext.ExecutingWebRequest +=
             delegate (object oSender, WebRequestEventArgs webRequestEventArgs)
             {
                 webRequestEventArgs.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + accessToken;
             };
        return clientContext;
    }

}
