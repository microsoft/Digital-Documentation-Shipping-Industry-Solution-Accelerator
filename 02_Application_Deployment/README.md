# Application Deployment

This folder contains documentation on how to run the Digital Documentation Shipping Industry Solution

 
## Prerequisites
Fundamental knowledge and access to [Visual Studio](https://visualstudio.microsoft.com/)


## Steps for adding Azure configuration to Client Application

To run the [source code](../01_Source_Code_Deployment/src):

1. Clone/download the [source code](./01_Source_Code_Deployment/src) onto your desktop and open in Visual Studio
2. Open the [ContosoCargo.DigitalDocument.TokenService.sln](../01_Source_Code_Deployment/src/ContosoCargo.DigitalDocument.TokenService.sln)
3. Navigate to WindowsApp > ContosoCargo.DigitalDocument.Application.WindowsClient > App.config
4. Replace HostKey and ServiceEndpoint with your Azure Functions values (e.g. _https://contosocargo.azurewebsites.net_**/api**)
5. Save and run the Windows Client as your start up project.
