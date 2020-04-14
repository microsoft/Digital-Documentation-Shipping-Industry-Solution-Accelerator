# Application Deployment

This folder contains a PowerShell script that can be used to provision the Azure resources required to build your Blockchain solution.  You may skip this folder if you prefer to provision your Azure resources via the Azure Portal.  The PowerShell script will provision the following resources to your Azure subscription:

 
| Resource              | Usage                                                                                     |
|-----------------------|-------------------------------------------------------------------------------------------|
| [Azure Blockchain Service](https://azure.microsoft.com/en-us/services/blockchain-service/) | Manage blockchain network deployments and operations|                                                     |
| [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/)  | The shipping transactions stored as a document          |
| [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)               | The API Host for shipping transactions                                                  |

## Prerequisites
1. Access to an Azure Subscription
2. Azure CLI Installed

## Deploy via Azure Portal
As an alternative to running the PowerShell script, you can deploy the resources manually via the Azure Portal or click the button below to deploy the resources:

<a href="https://azuredeploy.net/?repository=https:" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a> 

## Steps for Resource Deployment via PowerShell

To run the [PowerShell script](./deploy.ps1):

1. Modify the parameters at the top of **deploy.ps1** to configure the names of your resources and other settings.
2. Review the parameters set in the <resource>.parameters.json for each resource. To modify parameters you may:
    - Change the parameters directly in the file
    - Use additional parameters in the PowerShell script to override the ARM template parameters
3. Run the [PowerShell script](./deploy.ps1). If you have PowerShell opened to this folder run the command:
`./deploy.ps1`
4. You will then be prompted to login.
5. Log in to your Azure Subscription:
    - Verify the resources have been created (Azure Function deployment will also create an App Service Plan and Storage).
    - Copy the app name, host key and service endpoint in the Azure Functions
