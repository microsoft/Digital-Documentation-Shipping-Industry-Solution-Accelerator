$subscriptionID= '2851f2d7-fcd7-480d-b13d-b91fe8316aae'
$resourceGroupName = 'Shipping_Blockchain'
$resourceGroupLocation= 'SoutheastAsia'
$cmsbcLocation = 'JapanEast'

az login

az account set --subscription $subscriptionID

#create resource group
az group create `
    --location $resourceGroupLocation `
    --name $resourceGroupName `
    --subscription $subscriptionID

#create cosmos and blockchain accounts
#use the second paramters field to override resource location specified in template
az group deployment create `
   --name "deployment"  `
   --resource-group $resourceGroupName `
   --template-file cmsbcdeploy.json `
   --parameters cmsbcdeploy.parameters.json `
   --parameters cosmosLocation=${cmsbcLocation} bcLocation=${cmsbcLocation}

#create function app with serivce plan and and storage account
#location is the same as resource group
az group deployment create `
   --name "deploymentfunction" `
   --resource-group $resourceGroupName `
   --template-file fxappdeploy.json `
   --parameters fxappdeploy.parameters.json



Write-Host -NoNewLine 'Your deployment has finished, press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');