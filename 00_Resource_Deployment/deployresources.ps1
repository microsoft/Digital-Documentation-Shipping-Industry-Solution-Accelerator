$subscriptionID= '2851f2d7-fcd7-480d-b13d-b91fe8316aae'
$resourceGroupName = 'Shipping_Blockchain'
$resourceGroupLocation= 'SoutheastAsia'
$cmsbcLocation = 'JapanEast'

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
   --parameters cmsbcdeploy.parameters.json `
   --parameters cosmosLocation=${cmsbcLocation} bcLocation=${cmsbcLocation}

az group deployment create `
   --name "deploymentfunction" `
   --resource-group $resourceGroupName `
   --template-file fxappdeploy.json `
   --parameters fxappdeploy.parameters.json



Write-Host -NoNewLine 'Your deployment has finished, press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');