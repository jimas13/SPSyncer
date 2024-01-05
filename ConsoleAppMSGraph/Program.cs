
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using SPSyncer.Authentication;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
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

        //Define scopes. In this project we are targeting the Graph endpoint, hence the below scope.
        string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

        //initialize the graph service client which will help us with fetching the needed information.
        GraphServiceClient graphClient = new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
        {
            // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
            AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync();

            // Add the access token in the Authorization header of the API request.
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", result.AccessToken);
        }));

        //grab the list names contained in our "root" site collection
        var lists = await graphClient.Sites["root"].Lists
                                        .Request()
                                        .GetAsync();

        //print out the list names
        foreach (var list in lists)
        {
            if (list.DisplayName.Equals("Documents"))
            {
                Console.WriteLine(list.Name);
            }
        }

        //get Id of root site of sharepoint
        var site = await graphClient.Sites["root"].Request()
                                        .GetAsync();
        //get lists of the root site
        var filteredLists = await graphClient.Sites["root"].Lists
                                        .Request()
                                        .GetAsync();
        //filter out default Shared Documents Library from all of the lists of the Site
        var library = filteredLists.FirstOrDefault(tempList => tempList.DisplayName.Equals("Documents"));
        
        //define a file name
        var fileName = "SampleTextFile.txt";

        //define a file path
        var filePath = Path.Combine("C:\\path\\to\\document.txt");
        
        //open up stream for file contents
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            if (library is not null)
            {
                //get a target of the specified folder or better yet Drive.
                var targetFolder = graphClient.Sites[site.Id].Lists[library.Id].Drive.Root;
                // Upload a file
                var uploadedItem = await targetFolder.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(fileStream);
            }
        }
        Console.ReadKey();
    }
}