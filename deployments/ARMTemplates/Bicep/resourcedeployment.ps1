# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.

param(
    [Parameter(Mandatory= $True,
                HelpMessage='Enter the Azure subscription ID to deploy your resources')]
    [string]
    $subscriptionID = '',

    [Parameter(Mandatory= $True,
                HelpMessage='Enter the Azure Data Center Region to deploy your resources')]
    [string]
    $location = ''
)

Write-Host "Log in to Azure.....`r`n" -ForegroundColor Yellow
az login

az account set --subscription $subscriptionID
Write-Host "Switched subscription to '$subscriptionID' `r`n" -ForegroundColor Yellow

$deploymentName = 'DigitalShippingDeploy-' + ([string][guid]::NewGuid()).Substring(0,5)
$resourceGroupName = 'digitalshipping-' + ([string][guid]::NewGuid()).Substring(0,5)

$resourceGroup = az group exists -n $resourcegroupName
if ($resourceGroup -eq $false) {
    #create resource group
    az group create `
    --location $location `
    --name $resourceGroupName `
    --subscription $subscriptionID
}

Write-Host "Started deploying Digital Shipping resources.....`r`n" -ForegroundColor Yellow
$deploymentResult = az deployment group create -g $resourceGroupName --template-file .\main.bicep  -n $deploymentName
$joinedString = $deploymentResult -join "" 
$jsonString = ConvertFrom-Json $joinedString

$DShipUserIdentityID = $jsonString.properties.outputs.DShipUserIdentityID.value
$cosmosName  = $jsonString.properties.outputs.cosmosName.value
$functionAppName  = $jsonString.properties.outputs.functionAppName.value

Write-Host "--------------------------------------------`r`n" -ForegroundColor White
Write-Host "Deployment output: `r`n" -ForegroundColor White
Write-Host "Subscription Id: $subscriptionID `r`n" -ForegroundColor Yellow
Write-Host "Digital Shipping resource group: $resourcegroupName `r`n" -ForegroundColor Yellow
Write-Host "Cosmos DB Account: $cosmosName  `r`n" -ForegroundColor Yellow
Write-Host "User Assigned Identity Client Id: $DShipUserIdentityID `r`n" -ForegroundColor Yellow
Write-Host "Function App Name: $functionAppName `r`n" -ForegroundColor Yellow
Write-Host "--------------------------------------------" -ForegroundColor White


Write-Host "All resources are deployed successfully.....`r`n" -ForegroundColor Green