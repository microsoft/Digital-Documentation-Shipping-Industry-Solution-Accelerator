# Resource Deployment

This folder contains a PowerShell script that can be used to provision the Azure resources required to build your Blockchain solution.  You may skip this folder if you prefer to provision your Azure resources via the Azure Portal.  The PowerShell script will provision the following resources to your Azure subscription:

 
| Resource              | Usage                                                                                     |
|-----------------------|-------------------------------------------------------------------------------------------|
| [Azure Blockchain Service](https://azure.microsoft.com/en-us/services/blockchain-service/) | Manage blockchain network deployments and operations|                                                     |
| [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/)  | Document for shipping transactions          |
| *[Azure Functions](https://azure.microsoft.com/en-us/services/functions/)               | The API Host for shipping transactions                                                  |

*When you deploy the Azure Function, you will also need to create an [App Service Plan](https://azure.microsoft.com/en-us/pricing/details/app-service/plans/) and [Azure Storage account](https://azure.microsoft.com/en-us/services/storage/) associated with you [Azure Functions](https://azure.microsoft.com/en-us/services/functions/). If you choose to use the PowerShell script, these resources will be deployed automatically.

## Prerequisites
1. Access to an Azure Subscription
2. Azure CLI installed

## Deploy via Azure Portal
Please refer to the [Azure Availability](https://azure.microsoft.com/en-us/global-infrastructure/services/?products=functions,blockchain-service,cosmos-db&regions=all) website to ensure the deployed resources are available in your selected locations.

<a href="https://azuredeploy.net/?repository=https%3A%2F%2Fgithub.com%2Fmicrosoft%2FDigital-Documentation-Shipping-Industry-Solution-Accelerator%2Fblob%2Fmaster%2F00_Resource_Deployment" target="_blank">
  <img src="https://aka.ms/deploytoazurebutton"/>
</a>

## Steps for Resource Deployment via PowerShell

To run the [PowerShell script](./deploy.ps1):

1. Modify the parameters at the top of **deploy.ps1** to configure the names of your resources and other settings.
2. Review the parameters set in the <--resource-->.parameters.json for each resource. To modify parameters you may:
    - Change the parameters directly in the file
    - Use additional parameters in the PowerShell script to override the ARM template parameters.
3. Run the [PowerShell script](./deploy.ps1). If you have PowerShell opened to this folder run the command:
`./deploy.ps1`
4. You will then be prompted to login.
5. After the deployment is complete, log in to your Azure Subscription:
    - Verify the resources have been created.
6. Copy the following resource parameters:
    - Azure Functions Host Key (Function app settings > Host Key > master)
    - Azure Functions Service Endpoint (URL)
    - Cosmos DB Connection String
