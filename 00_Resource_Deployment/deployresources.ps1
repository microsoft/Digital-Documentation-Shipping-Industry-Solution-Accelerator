$subscriptionID= '2851f2d7-fcd7-480d-b13d-b91fe8316aae'
$resourceGroupName = 'Digital_Shipping_Blockchain'
$resourceGroupLocation= 'SouthCentralUS'

az login

az account set --subscription $subscriptionID

az group create `
    --location $resourceGroupLocation `
    --name $resourceGroupName `
    --subscription $subscriptionID

az group deployment create `
   --name "deployment"  `
   --resource-group $resourceGroupName `
   --template-file cmsbcdeploy.json `
   --parameters cmsbcdeploy.parameters.json

az group deployment create `
   --name "deploymentfunction" `
   --resource-group $resourceGroupName `
   --template-file fxappdeploy.json `
   --parameters Join-Path fxappdeploy.parameters.json