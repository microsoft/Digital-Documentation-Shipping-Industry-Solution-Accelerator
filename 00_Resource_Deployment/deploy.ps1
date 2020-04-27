# These values need to be altered before deployment
 $subscriptionID = '<subscription ID>'
$resourceGroupName = 'DigitShiptBlockchain'
$resourceGroupLocation= 'CentralUS'
$resourceName = 'digitalship'


# These values can be used to override resource parameters specified in template.
# (e.g.    --parameters bcLocation=${bcLocation})

# Default ARM template parameters:
# - resourceName = 'contosocargo',
# - cmsLocation = 'West US 2'
# - bcLocation = 'West US 2'
# - fxLocation = 'Central US'


az login

az account set --subscription $subscriptionID

#create resource group
az group create `
    --location $resourceGroupLocation `
    --name $resourceGroupName `
    --subscription $subscriptionID

#create cosmos account
az deployment group create `
   --name "cmsdeployment"  `
   --resource-group $resourceGroupName `
   --template-file cosmosdeploy.json `
   --parameters cosmosdeploy.parameters.json `
   --parameters resourceName=${resourceName}

#create blockchain account
az deployment group create `
   --name "bcdeployment"  `
   --resource-group $resourceGroupName `
   --template-file blockchaindeploy.json `
   --parameters blockchaindeploy.parameters.json `
   --parameters resourceName=${resourceName}

#create function app with serivce plan and and storage account
#location is the same as resource group
az deployment group create `
   --name "fxdeployment" `
   --resource-group $resourceGroupName `
   --template-file fxappdeploy.json `
   --parameters fxappdeploy.parameters.json `
   --parameters resourceName=${resourceName}



Write-Host -NoNewLine 'Your deployment has finished, press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');