## Resource Deployment
The [PowerShell script](./Bicep/resourcedeployment.ps1) can be used to provision the Azure resources required to deploy this Digital Documentation for Shipping Industry Solution Accelerator. 
You may skip this section if you prefer to provision your resources via the Azure Portal, using the Azure Resource Manager(ARM) Template provided, or one of the Deploy to Azure buttons on the main documentation page.

The PowerShell script will provision the following resources to your Azure subscription:
- Azure Cosmos DB API for MongoDB account
- Azure Function App
- Azure App Service Plan
- Azure Storage Account
- Azure Managed Identity

## Prerequisites
 1. [Azure Subscription](http://portal.azure.com) - Required to deploy compute resources
 2. [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1) - Required to run deployment scripts
 3. [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed - Required to run deployment scripts
 4. [User Access Administrator](https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#user-access-administrator) Role - Assigned to Azure Subscription user

Execute the following steps to deploy Azure resources:

## Step 1. Download Files
Clone or download this repository, if you have not already done so.

Check here for more information on [cloning a repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository).

## Step 2. Deploy Digital Shipping Resources
1. Run [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)

2. Run the Change Directory command to navigate to the location where **resourcedeployment.ps1** is located
    ```console
    PS C:\Users\>CD <directory path>
    ```
    **Remember to write down all of the output values printed on the screen. These will be required in the next step (when deploying Azure Function code).**

3. Run the **resourcedeployment.ps1** with the following parameters:
```.\resourcedeployment.ps1 <SubscriptionId> <location>```

    ```
    SubscriptionId: The subscription ID for where you want to manage your resources
    location: Azure Data Center Region where resources will be deployed
    ```

    - If you see this error message when running the PowerShell script:

        ![alt text](/documents/media/ResourceDeploymentError.png)

        - Resolve it by running the following command:
         ```Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass```

         - Then run ```.\resourcedeployment.ps1 <SubscriptionId> <location>```


After the completion of the script, check to see that all of the Azure resources deployed successfully. Your resource groups should look similar to the image below.

![alt text](/documents/media/Resources.png)

## Step 3. Configure and Assign Managed Identity 

- **Reminder:** The managed identity name will differ by deployment. Your managed identity name will be different. Ex. DShipUserIdentity-XXXXX

#### Assign Managed Identity to Azure Function
1. Step into the Azure Function app

    ![alt text](/documents/media/FunctionApp.png)  


2. Under the settings click on the identity

    ![alt text](/documents/media/FunctionAppIdentity.png)  


3. Click on the User Assigned tab and click on add and select DShipUserIdentity-XXXXX

    ![alt text](/documents/media/FunctionAppIdentityAdd.png)


4. Refresh to confirm identity assignment
 
    ![alt text](/documents/media/FunctionAppIdentityVerify.png)


#### Assign Managed Identity to Azure Cosmos DB
1. Step into the Azure Cosmos DB account

    ![alt text](/documents/media/CosmosDb.png) 

2. Click on the Access control and click on add and select Add role assignment

    ![alt text](/documents/media/CosmosDbAccess.png)

3. Search for the "DocumentDB Account Contributor" in the search box given. Select the "DocumentDB Account Contributor" role and click Next

    ![alt text](/documents/media/CosmosDbSelect.png)

4. Select "Managed Identity," and click on the "Selected members," and select the identity you configured the steps above
    ![alt text](/documents/media/CosmosDbIdentitySelect.png)

5. Click on "Review + assign" to add the role assignment

    ![alt text](/documents/media/CosmosDbIdentityAssign.png)

6. Refresh to confirm role assignment
 
    ![alt text](/documents/media/CosmosDbIdentityVerify.png)


**You've successfully deployed all the resources!**

For the next step, go to [Application Deployment](/deployments/AzureFunctions/README.md).