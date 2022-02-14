# Application Deployment

## Prerequisites
To successfully deploy the digital shipping industry azure functions, you will need to have deployed the following resources.
 
 1. Azure Storage Account
 2. Azure Cosmos DB API for Mongo DB Account
 3. Azure Function App
 4. Azure App Service Plan
 5. Azure Managed Identity
 6. Azure CLI installed
 7. [Azure Core Tool installed](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v4%2Cwindows%2Ccsharp%2Cportal%2Cbash#v2)
 8. Azure Non-Fungible Token Solution Accelerator

If these are not available, please follow the [resource deployment](../ARMTemplates/ResourceDeployment.md) steps. 

## Steps for Azure Functions Deployment via PowerShell

### Step 1. Setup Azure core tool globally. (Required first time only)
1. Open the PowerShell tool and run the below command.
    ```
    npm install -g azure-functions-core-tools@4
    ```
### Step 2. Execute the PowerShell deployment script.
1. Run [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)
2. Run the Change Directory command to navigate to the location where **deployazurefunction.ps1** is located
    ```console
    PS C:\Users\>CD <directory path>
    ```
3. Provide the following detail in deployazurefunction.ps1 from the previous script(resourcedeployment.ps1) output
    ```
    SubscriptionId: The Subscription ID for where you want to manage your resources
    ResourcegroupName: Resource group name where the resources are deployed
    DatabaseAccountName : Azure Cosmos DB account name
    serviceEndpointUrl: NFT service endpoint URL received after deployment of Azure Non Fungible Token Solution Accelerator.
    ManagedIdentityId: User Assigned Identity Client Id
    FunctionAppName: Name of app that requires Azure Functions support
    ```
4. Run the .\deployazurefunction.ps1
    ```console
    PS  <directory path> \deployments\AzureFunctions>Powershell.exe -executionpolicy remotesigned -File .\deployazurefunction.ps1
    ```
5. Accept the log-in request through your browser

6. After the successful execution of the script, the Azure Functions will be deployed.

    
    **You've successfully deployed Azure Functions!**
    
    **Note:** Azure function URL and Hostkey will be received after successful deployment. Copy this detail as it will be required in next step.
    
    ![alt text](/documents/media/AzureFunctionURLandHostkey.png)

For next steps, please go to [**Setup Console Application**](/documents/README.md).

---

## Detail of the deployment script
1. Log in to the Azure portal

    ```
    az login
    ```
2. Set the Azure account Subscription ID

    ```
    az account set --subscription mysubscriptionid
    ```
3. Set Up Azure function application

    ```
    Set-location "..\..\src"

    ((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ServiceEndpoint}', $serviceEndpointUrl) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json

    ((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{SubscriptionId}', $subscriptionId) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json

    ((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ResourceGroupName}', $resourcegroupName) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json

    ((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{DatabaseAccountName}', $databaseAccountName) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json

    ((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ManagedIdentityId}', $ManagedIdentityId) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json
    ```
5. Deploy Azure Functions
    ```
    Set-location "ContosoCargo.DigitalDocument.TokenService.Host"
    func azure functionapp publish $FunctionAppName --csharp --force
    Set-Location "..\..\deployments\AzureFunctions"
    ```
6. Update function App configuration

    ```
    az functionapp config appsettings set --name $FunctionAppName --resource-group $resourcegroupName --settings "FUNCTIONS_WORKER_RUNTIME=dotnet-isolated"
    ```