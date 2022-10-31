# SPSyncer
Three similar solutions which achieve communication with the SharepointOnline service from a deamon console app.


#ConsoleApp1
This directory contains a PnP.Framework sample that shows how to get a client context to a SharePoint Online Site collection with a modern approach.

#ConsoleAppMSGraph
This directory contains a MSGraph project sample that utilizes the GraphServiceClient in order to fetch infrormation about a site collection.

#ConsoleAppSPClient
Default way of accesing a SharePoint Site Collection by utilizing the Microsoft.SharePoint.Client namespace.

In order for the three above way to function as intended the below are required:
- An App registration in the tenant's Azure Active directory
- The below API Permissions: 
  ![image](https://user-images.githubusercontent.com/17068157/199127969-666cefb5-d605-4a7e-a6cb-b728827b69b7.png)
