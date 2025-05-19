#!/bin/bash

# This script creates a service principal for Azure authentication
# and configures the necessary permissions for CI/CD

# Variables
SERVICE_PRINCIPAL_NAME="sp-tpg-cicd"
SUBSCRIPTION_ID=$(az account show --query id -o tsv)

# Create service principal and save credentials
echo "Creating service principal..."
credentials=$(az ad sp create-for-rbac --name $SERVICE_PRINCIPAL_NAME \
  --role Contributor \
  --scopes /subscriptions/$SUBSCRIPTION_ID \
  --sdk-auth)

echo "Service principal created successfully!"
echo "--------------------------------------"
echo "To use in GitHub Actions, configure the following secret:"
echo "AZURE_CREDENTIALS: $credentials"
echo ""
echo "Individual values can be extracted as needed:"
echo "CLIENT_ID: $(echo $credentials | jq -r .clientId)"
echo "CLIENT_SECRET: $(echo $credentials | jq -r .clientSecret)"
echo "TENANT_ID: $(echo $credentials | jq -r .tenantId)"
echo "SUBSCRIPTION_ID: $(echo $credentials | jq -r .subscriptionId)"
