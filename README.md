# SPSyncer
Three similar solutions which achieve communication with the SharepointOnline service from a deamon console app.


# ConsoleApp1

This directory contains a PnP.Framework sample that shows how to get a client context to a SharePoint Online Site collection with a modern approach.

# ConsoleAppMSGraph

This directory contains a MSGraph project sample that utilizes the GraphServiceClient in order to fetch infrormation about a site collection.

# ConsoleAppSPClient

Default way of accesing a SharePoint Site Collection by utilizing the Microsoft.SharePoint.Client namespace.

# Minimal Path to Awesome

In order for the three above way to function as intended the below are required:
- An App registration in the tenant's Azure Active directory
- The below API Permissions: 

  ![image](https://user-images.githubusercontent.com/17068157/199128217-79dbcf79-835d-498e-8491-e18f447caa89.png)

** Bear in mind, that the Application Permission Sites.Selected could also be picked, but that would require additional PS scripting that is not in the scope of this example
