---
page_type: sample
languages:
- csharp
name: ".NET Core Azure Function to send SMTP email to/with Office 365"
description: "Azure Function that receives a POST request then sends email."
products:
- azure
- dotnet-core
- office-365
- office-exchange-server
- office-outlook
services:
- azure-functions
---

# Azure .NET Core Function to send email through SMTP for Office 365

Created to send email via an Azure Function, using .NET Core, through Office 365. 

## Setup

* Clone repo
* Publish Azure Function to Azure Functions
* Get Azure Function's Function Key - required to access function

## To Use

* Edit `data.json`, found in root of this project, with your own values:

	```json
	{
	  "toEmail": "johnsmith@contoso.com",
	  "textSubject": "testing Azure function",
	  "textBody": "hello body",
	  "fromAccountEmail": "<REPLACE-WITH-YOUR-0365-ACCOUNT>@microsoft.com",
	  "fromAccountPassword": "<REPLACE-WITH-YOUR-0365-ACCOUNT-PASSWORD>",
	  "smtpHost": "smtp.office365.com",
	  "smtpPort": "587",
	  "fromAccountDomain": "microsoft.com"
	}
	```

* Example HTTP command: 

    ```http
    POST /api/sendemail?code=<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-FUNCTION-KEY> HTTP/1.1
	Host: <REPLACE_WITH-YOUR-FUNCTION-RESOURCE-NAME>.azurewebsites.net
	Content-Type: application/json
	Cache-Control: no-cache

	{
		toEmail: "johnsmith@contoso.com",
		textSubject: "testing Azure function",
		textBody: "hello body",
		fromAccountEmail:"<REPLACE-WITH-YOUR-0365-ACCOUNT>@microsoft.com",
		fromAccountPassword:"<REPLACE-WITH-YOUR-0365-ACCOUNT-PASSWORD>"
		smtpHost: "smtp.office365.com"
		smtpPort: "587"
	}
    ```

	Use the table to understand the replacements:

	|Term|Replacement|
	|--|--|
	|`<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-NAME>`|String: your Azure Function resource name|
	|`<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-FUNCTION-KEY>`|String: your Azure Function resource's function key. The function returns auth error if the key is not sent.|
	|`<REPLACE-WITH-YOUR-0365-ACCOUNT>`|String: your 0365 User account.|
	|`<REPLACE-WITH-YOUR-0365-ACCOUNT-PASSWORD>`|String: your 0365 User account password.|

	Execute the cURL command from a bash terminal in the root of this project so that the `data.json` file doesn't need any path resolution. Make sure to change the following command to use your Azure Function name and key

	```CURL
	curl -v -X POST \
	-H 'Content-type: application/json' \
	-d @data.json 'https://<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-NAME>.azurewebsites.net/api/sendemail?code=<REPLACE_WITH-YOUR-FUNCTION-RESOURCE-FUNCTION-KEY>'
	```

## Caveats

This Azure function can't be successfully run from a local machine due to the restrictions of the SMTP client usage from Office 365. See [Documentation references](#documentation-references) for details. 

## Watch Azure Function Log stream

When you test this Azure-deployed function, use the [Azure portal](https://portal.azure.com) to watch this Azure Function's Log stream. Successful logging output looks something like:

```console
2021-07-26T16:00:30.000 [Information] Executing 'SendEmail' (Reason='This function was programmatically called via the host APIs.', Id=dde6166f-8833-4a7f-b40d-c9c7d2fc48ca)
2021-07-26T16:00:30.000 [Information] sendemail
2021-07-26T16:00:30.001 [Information] sendemail received data
2021-07-26T16:00:30.001 [Information] sendemail fetched variables
2021-07-26T16:00:30.004 [Information] sendemail got variables
2021-07-26T16:00:30.005 [Information] sendemail client constructed
2021-07-26T16:00:30.005 [Information] sendemail mail constructed
2021-07-26T16:00:31.373 [Information] sendemail mail sent
2021-07-26T16:00:31.374 [Information] Executed 'SendEmail' (Succeeded, Id=dde6166f-8833-4a7f-b40d-c9c7d2fc48ca, Duration=1374ms)
```

## Documentation references
	
* [How to set up SMTP AUTH client submission](https://docs.microsoft.com/en-us/Exchange/mail-flow-best-practices/how-to-set-up-a-multifunction-device-or-application-to-send-email-using-microsoft-365-or-office-365?redirectSourcePath=%252fen-us%252farticle%252fHow-to-set-up-a-multifunction-device-or-application-to-send-email-using-Office-365-69f58e99-c550-4274-ad18-c805d654b4c4)
* [5.7.57 SMTP - Client was not authenticated to send anonymous mail during MAIL FROM error](https://stackoverflow.com/questions/30342884/5-7-57-smtp-client-was-not-authenticated-to-send-anonymous-mail-during-mail-fr)
	
- ...
