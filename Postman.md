# How To configure Postman for NLayers WebApi

This article describes how to configure the Postman REST client to interact with and test the **SGS NLayers APIs**. Specifically, it describes:

- How to import the swagger.json file from your SGS NLayers APIs solution to Postman.
- How to use the Postman REST client to make token-bearing HTTP requests to your Management APIs.

## Postman summary

Get started on **SGS NLayers APIs** by using a REST client tool such as [Postman](www.getpostman.com) to prepare your local testing environment. The Postman client helps to quickly create complex HTTP requests. Download the desktop version of the Postman client by going to [www.getpostman.com/apps](www.getpostman.com/apps).

# Import the swagger.json file to Postman

1. Open your SGS NLayers solution with Visual Studio 2019, set the Host project as your startup project and press F5.

2. Copy and Paste this URL [https://localhost:44321/swagger/1.0/swagger.json](https://localhost:44321/swagger/1.0/swagger.json) in the address bar of your navigator and save the .json file on your machine

3. Open Postman and select `Import` button from the upper-left side of the window, from the `Import file` tab select `Choose files` button and select the *swagger.json* file that you has saved from the previous point in your machine.

4. The imported collection will appear in the navidation menu.

# Obtain an OAuth 2.0 token

Set up and configure Postman to obtain an Azure Active Directory token. Afterwards, make an authenticated HTTP request to **SGS NLayers APIs** using the acquired token:

1. Go to [www.getpostman.com/apps](www.getpostman.com/apps) to download the app.

2. Verify that your Authorization URL is correct. It should take the format
``` console
https://login.microsoftonline.com/YOUR_AZURE_TENANT.onmicrosoft.com/oauth2/authorize
```
| Name                  | Replace with                            | Example         |
| :-------------------  | :---------------------------------------|:----------------|
| YOUR_AZURE_TENANT     | The name of your tenant or organization | sgs             |
|

3. Select the **Authorization** tab, select **OAuth 2.0**, and then select **Get New Access Token**.

| Field                 | Value                                                     | 
| :-------------------  | :---------------------------------------------------------|
| Grant Type            | Implicit |
| Callback URL          | http://localhost:4200/                                    |
| Auth URL              | Use the **Authorization** URL from **step 2**             |
| Client ID             | Use the Application ID for the Azure Active Directory app (*b480a076-eab2-4af2-ba8e-4234a75ecdab*)                                            |
| Scope                 | api://9c54949a-7e4a-402d-9912-1fa840360a9f/access_as_user |
| State                 | Leave blank                                               |
| Client Authentication | Send as Basic Auth header                                 |
|

> The values show above are from our Azure Active Directory test apps and are within a isolated tenant.

4. Select **Request Token**.

5. Scroll down and select **Use Token**.

