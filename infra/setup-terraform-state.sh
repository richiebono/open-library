# Script to configure Terraform state storage

# This script configures the Azure Storage Account to store the Terraform state
# It should be run only once before using Terraform for the first time

# Variables
RESOURCE_GROUP_NAME="rg-terraform-state"
STORAGE_ACCOUNT_NAME="tpgterraformstate"
CONTAINER_NAME="tfstate"
LOCATION="eastus"

# Login to Azure (if not already authenticated)
echo "Checking Azure authentication..."
az account show &> /dev/null || az login

# Create resource group
echo "Creating Resource Group for Terraform state..."
az group create --name $RESOURCE_GROUP_NAME --location $LOCATION

# Create storage account
echo "Creating Storage Account..."
az storage account create \
  --resource-group $RESOURCE_GROUP_NAME \
  --name $STORAGE_ACCOUNT_NAME \
  --sku Standard_LRS \
  --encryption-services blob

# Get access key
echo "Getting access key..."
ACCESS_KEY=$(az storage account keys list \
  --resource-group $RESOURCE_GROUP_NAME \
  --account-name $STORAGE_ACCOUNT_NAME \
  --query '[0].value' -o tsv)

# Create container
echo "Creating container to store the state..."
az storage container create \
  --name $CONTAINER_NAME \
  --account-name $STORAGE_ACCOUNT_NAME \
  --account-key $ACCESS_KEY

echo "Terraform state configuration completed!"
echo "----------------------------------------"
echo "To use in GitHub Actions, configure the following secrets:"
echo "TF_STATE_STORAGE_ACCOUNT_NAME: $STORAGE_ACCOUNT_NAME"
echo "TF_STATE_CONTAINER_NAME: $CONTAINER_NAME"
echo "TF_STATE_ACCESS_KEY: $ACCESS_KEY"
