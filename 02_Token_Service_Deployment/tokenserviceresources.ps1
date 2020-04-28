$resourceGroupName = 'DigitShiptBlockchain'
$location = 'CentralUS'
$subscriptionID = '<subscription ID>'
$containerregistry = 'contosocargoacregistry'
$kuberneteservice = 'contosocargok8s'
$keyvaultname = 'shippingkeyvault11'

az login
az account set --subscription $subscriptionID

az group create `
    --location $location `
    --name $resourceGroupName `
    --subscription $subscriptionID

 # Create a Container Registry   
az acr create --name $containerregistry --resource-group $resourceGroupName --location $location --subscription $subscriptionID --sku "Standard"

 # Create a Kubernetes Service
az aks create --resource-group $resourceGroupName --name $kuberneteservice --node-count 1 --enable-addons monitoring --generate-ssh-keys

 # Create a Key Vault   
az keyvault create --name $keyvaultname --resource-group $resourceGroupName --location $location --subscription $subscriptionID

 # Create a Service Principal
$credentials = $(az ad sp create-for-rbac --role "Contributor" --scopes "/subscriptions/$subscriptionID/resourceGroups/$resourceGroupName" -o json)

 # Save SP Credentials
$jsonCredentials = $credentials | ConvertFrom-Json
$appId = $jsonCredentials.appId
$password = $jsonCredentials.password
$tenant = $jsonCredentials.tenant

# Update Key Vault Policy to accept Service Principal request
az keyvault set-policy -n $keyvaultname --spn $appId --key-permissions create decrypt delete import update encrypt get list unwrapKey wrapKey


 # Check access to Service Principal
az login --service-principal -u $appId --password $password --tenant $tenant

$credentials | Out-File -FilePath .\Credentials.txt

Write-Host -NoNewLine 'Your deployment has finished, press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');