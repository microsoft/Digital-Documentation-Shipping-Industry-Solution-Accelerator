# Token Service Deployment

This folder contains a YAML script that can be used to upload functions into your [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) resource that you deployed in the previous step. The diagram below shows how the source code we deploy will interact with the other solution components.

 
## Prerequisites
Fundamental knowledge and access to [Azure Pipelines](https://azure.microsoft.com/en-us/services/devops/pipelines/)
Fundamental knowledge of the following Azure resources*:
[Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/)
[Azure Kubernetes Service](https://azure.microsoft.com/en-us/services/kubernetes-service/)
[Service Principals](https://docs.microsoft.com/en-us/azure/active-directory/develop/app-objects-and-service-principals)
[Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/)

*These resources should be deployed before you start and in the same group as the resources deployed in (../00_Resource_Deployment)

For **quick** and **easy** deployment, feel free to use the [PowerShell script](./tokenserviceresources.ps1) in this folder and make sure the $resourceGroupName matches your other resources.

## Steps for Source Code Deployment via Azure Pipelines

To run the [Pipeline](./azure-pipelines.yml):


1. Copy the [src](./src) folder and [pipeline script](./azure-pipelines.yml) into an [Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) repository.
2. Create a new pipeline and connect it to the repository.
3. Under Pipelines, add a New Environment:
    - Resource: Kubernetes, click "Create".
    - Provider: Azure Kubernetes Service
    - Select your desired subscription and ACR resource, click "Validate and Create"
4. Create a Docker Registry service connection:
    - Navigate to Project Settings (bottom left) > Service connections
    - Create a New service connection, select Docker Registry
    - Registry type: Azure Container Registry
    - Select your desired subscription and ACR resource, click "Save"
    - Click on the service connection, copy the resourceId in the URL (GUID)
5. Change and save the image parameter in [deployment.yml](./deployment.yml) (manifests > deployment.yml):
    - image: <--container login server-->/abtoptionb
6. Change and save the parameters in [azure-pipelines.yml](./azure-pipelines.yml):
    - dockerRegistryServiceConnection: <--service connection resourceId -->
    - containerRegistry: <--container login server -->
    - environment: <--environmentName-->.<--namespace-->
7. Input Azure connections into [appsettings.json](./src/Microsoft.TokenService.API/appsettings.json) (src > TokenService.API > appsettings.json).
    - Offchain_Connectionstring: <--primary connection string for cosmos DB-->
    - KeyVaultURL: <--key vault URL-->
    - ClientID: <--appID--> ClientSecret: <--password-->*
    *If you run the PowerShell script, the RBAC values are saved in an outpot (Credentials.txt).
8. Run the pipeline and when it is finished, copy your service endpoint External IP and port:
    - Pipelines > Environments > (environment) > (k8s resource) > Services > abtoptionb > External IP & port

## Register your Azure Blockchain Service and Users to your Token Service endpoint

You will need to create networks, parties, and users in order to run the application. To make the necessary API calls you can use the following tools.
    - Swagger
        1. Open a browser
        2. Type in the URL <--External IP-->/swagger)
    - PostMan
        1. From [documents](./documents), import the collections and environments files into [PostMan](https://www.postman.com/)
        2. Next to the Environment label in the top right to select ProjectOptionB and click the eye to change the following values:
            - serviceendpoint: <--External IP-->
            - port: <--port-->
            - protocol: http

2. Register Blockchain Network Go to /ServiceManagement/BlockchainNetworks/BlockchainNetwork and send a request with the following:
    - name of your network
    - node URL
    - description if desired 
3. To create a 

## Use PostMan to send API requests


3. Use API in imported collection to send request to your service endpoint

### Running your Token Service locally

You may also choose to run the token service locally. If you choose to do this in conjunction with the client application, you will need to have two instances of Visual Studio running. To run the solution, set TokenService.API as your start up project and use the URL in the browser to navigate to your swagger page; you may also use the swagger collection and environment to interact with the local endpoint. To see a demo of the blockchain mechanisms, run the TokenClientSample project.