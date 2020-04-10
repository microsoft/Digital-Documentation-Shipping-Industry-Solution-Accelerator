# Source Code Deployment

This folder contains a YAML script that can be used to upload functions into your [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) resource that you deployed in the previous step.

![Microservices Architecture](./References/architecture_function.jpg)
 
## Prerequisites
Fundamental knowledge and access to [Azure Pipelines](https://azure.microsoft.com/en-us/services/devops/pipelines/)


## Steps for Source Code Deployment via Azure Pipelines

To run the [Pipeline](./azure-pipelines.yml):

1. Select where your code is coming from, the easiest way to run the pipeline is to copy the solution into an [Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) repository.
2. Find the repository where the pipeline is stored and select the [pipeline](./01_Source_Code_Deployment/azure-pipelines.yml)
3. The page should now be opened to the pipeline, double click on the settings header the task 'AzureFunctionApp@1'
4. In the right panel pop-up, fill-out: subscription, ...
