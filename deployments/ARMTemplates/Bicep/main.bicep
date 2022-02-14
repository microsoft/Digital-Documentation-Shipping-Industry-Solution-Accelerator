// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

@description('Provide a location to deploy your resources')
var location = resourceGroup().location

@description('Cosmos DB account name, can only contain lowercase letters, numbers, and the hyphen (-) character. It must be between 3-44 characters in length')
var cosmosAccountName = 'cosmos-contosocargo-${take(uniqueString(resourceGroup().id),5)}'

@description('Provide a name for function app')
var funcAppName = 'func-contosocargo-${take(uniqueString(resourceGroup().id),5)}'

@description('Storage account name, only lowercase letters and numbers, Name must be between 3 and 24 characters')
var funcStorageName = 'stcontosocargo${take(uniqueString(resourceGroup().id),5)}'

@description('Provide a name for the app service plan')
var serverFarmName = 'plan-contosocargo-${take(uniqueString(resourceGroup().id),5)}'

@description('Managed Idenity name')
var DigitShipUserIdentity = 'DShipUserIdentity-${take(uniqueString(resourceGroup().id),5)}'

resource cosmosAccountName_resource 'Microsoft.DocumentDB/databaseAccounts@2021-10-15' = {
  name: cosmosAccountName
  location: location
  kind:'MongoDB'
  properties: {
    enableAutomaticFailover: false
    enableMultipleWriteLocations: false
    isVirtualNetworkFilterEnabled: false
    virtualNetworkRules: []
    databaseAccountOfferType: 'Standard'
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
      maxIntervalInSeconds: 5
      maxStalenessPrefix: 100
    }
    locations: [
      {
        locationName: location
        failoverPriority:0
        isZoneRedundant:false
      }
    ]
    capabilities:[
      {
        name: 'EnableMongo'
      }
    ]
  }
}


resource funcStorageName_resource 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: funcStorageName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}

resource funcAppName_resource 'Microsoft.Web/sites@2021-02-01' = {
  name: funcAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: serverFarmName_resource.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageName};AccountKey=${listKeys(funcStorageName, '2021-04-01').keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${funcStorageName};AccountKey=${listKeys(funcStorageName, '2021-04-01').keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: funcAppName
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'WEBSITE_ENABLE_SYNC_UPDATE_SITE'
          value: 'true'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
    }
  }
  dependsOn: [
    funcStorageName_resource
  ]
}

resource serverFarmName_resource 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: serverFarmName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource DShipUserIdentity_resource 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: DigitShipUserIdentity
  location: location
}

resource traceTag_resource 'Microsoft.Resources/deployments@2021-04-01' = {
  name: 'pid-b467f153-9bc9-5631-adb5-3eefa231a547'
  properties:{
    mode: 'Incremental'
    template:{
      '$schema': 'https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#'
      contentVersion: '1.0.0.0'
      resources: []
    }
  }
}

output DShipUserIdentityID string = DShipUserIdentity_resource.properties.clientId
output cosmosName string = cosmosAccountName_resource.name
