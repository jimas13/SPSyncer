using System.Net;
using SPSyncer.Authentication;

//read and bind the configuration to an strongly typed object for ease of access
AuthenticationConfiguration config = AuthenticationConfiguration.ReadFromJsonFile("appsettings.json");

//initialize the Authentication manager with the information from the appsettings configuration file.
var authManager = new PnP.Framework.AuthenticationManager(config.ClientId, config.Certificate.CertificateDiskPath, config.Certificate.CertificatePassword, config.Tenant);

//Use the GetAccessTokenContext and define an anonymous lamda function which will help us with the token aquisition
using (var cc = authManager.GetAccessTokenContext("https://<REPLACE:name of tenant>.sharepoint.com/sites/testsite2",
                                                (string siteURL) => authManager.GetAccessToken(siteURL)))
{
    //load our Web object and print out its title property.
    cc.Load(cc.Web, p => p.Title);
    await cc.ExecuteQueryAsync();
    Console.WriteLine(cc.Web.Title);
    Console.ReadKey();
}
