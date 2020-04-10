$subscriptionID= '036cb61a-ccf5-49ea-90fc-391eb7720410'
$resourceGroupName = 'Digital_Shipping_Blockchain'
$resourceGroupLocation= 'KoreaSouth'

az login

az account set --subscription $subscriptionID

#create resource group
az group create `
    --location $resourceGroupLocation `
    --name $resourceGroupName `
    --subscription $subscriptionID


#create function app with serivce plan and and storage account
#location is the same as resource group
az group deployment create `
   --name "fxdeployment" `
   --resource-group $resourceGroupName `
   --template-file fxappdeploy.json `
   --parameters fxappdeploy.parameters.json



Write-Host -NoNewLine 'Your deployment has finished, press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');