# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.
#login azure
Write-Host "Log in to Azure"
az Login

$subscriptionId = Read-Host "subscription Id"
$resourcegroupName = Read-Host "resource group name"
$databaseAccountName = Read-Host "azure cosmos db account name"
$serviceEndpointUrl = Read-Host "NFT service end point URL"
$ManagedIdentityId = Read-Host "user assigned identity client id"
$FunctionAppName = Read-Host "function app name"

Write-Host "Setup subscription Id...`r`n"

az account set --subscription $subscriptionId

$resourceGroup = az group exists -n $resourcegroupName
if ($resourceGroup -eq $false) {
    throw "The Resource group '$resourcegroupName' is not exist`r`n Please check resource name and try it again"
}

Write-Host "Setup Azure Functions Appsetting.....`r`n"

Set-location "..\..\src"

((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ServiceEndpoint}', $serviceEndpointUrl) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json
((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{SubscriptionId}', $subscriptionId) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json
((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ResourceGroupName}', $resourcegroupName) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json
((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{DatabaseAccountName}', $databaseAccountName) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json
((Get-Content -path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json -Raw) -replace '{ManagedIdentityId}', $ManagedIdentityId) | Set-Content -Path ContosoCargo.DigitalDocument.TokenService.Host\application.settings.json

Write-Host "Deploy Azure Functions `r`n"

Set-location "ContosoCargo.DigitalDocument.TokenService.Host"

func azure functionapp publish $FunctionAppName --csharp --force

Set-Location "..\..\deployments\AzureFunctions"

Write-Host "Update function App configuration.....`r`n"

az functionapp config appsettings set --name $FunctionAppName --resource-group $resourcegroupName --settings "FUNCTIONS_WORKER_RUNTIME=dotnet-isolated"

Write-Host "**You've successfully deployed the Azure Functions!**" -ForegroundColor Green

az functionapp list --query "[?name == '$FunctionAppName'].{HostName: defaultHostName}" --output table
Write-Host "";
az functionapp keys list --name $FunctionAppName -g $resourcegroupName --query "{FunctionKey : functionKeys.default }" --output table