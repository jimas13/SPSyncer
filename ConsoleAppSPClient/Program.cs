using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.SharePoint.Client;
using SPSyncer.Authentication;
using ConsoleAppSPClient.Helper;

//define the site URL that we wish to target through our context
string siteURL = "https://<REPLACE: name of tenant>.sharepoint.com/sites/<REPLACE:some site name>";

//define scopes of our request. Pay attention on how the resource is depicted for sharepoint clients
string[] scopes = new string[] { "https://<REPLACE:name of tenant>.sharepoint.com/.default" };

//read and bind the configuration to an strongly typed object for ease of access
AuthenticationConfiguration config = AuthenticationConfiguration.ReadFromJsonFile("appsettings.json");

//grab the certificate in order to fullfill our request
CertificateDescription Certificate = config.Certificate;
ICertificateLoader certificateLoader = new DefaultCertificateLoader();
certificateLoader.LoadIfNeeded(Certificate);

//initialize the Confidential Client Application which will fetch us the token
IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
    .WithCertificate(Certificate.Certificate)
    .WithTenantId(config.Tenant)
    .WithAuthority(config.Authority)
    .Build();

// Retrieve an access token for SharepointOnline (gets a fresh token if needed).
var result = await app.AcquireTokenForClient(scopes)
.ExecuteAsync();

//Use our helper method in order to include the previously generated token in our requests to AAD and Sharepoint Online, subsequently
using (var clientContext = ContextHelper.GetClientContext(siteURL, result.AccessToken))
{
    //Load the Web object of our targeted site and print out its Title.
    Web web = clientContext.Web;
    clientContext.Load(web);
    await clientContext.ExecuteQueryAsync();
    Console.WriteLine(web.Title);

}
