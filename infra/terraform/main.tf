terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.85.0"
    }
  }

  backend "azurerm" {
    # These values will be filled during initialization with the secret values
  }

  required_version = ">= 1.5.0"
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

# Create Resource Group if it doesn't exist
resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.location
}

# Log Analytics Workspace for Container App metrics and logs
resource "azurerm_log_analytics_workspace" "main" {
  name                = "log-${var.container_app_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

# Azure Container Registry if it doesn't exist
resource "azurerm_container_registry" "acr" {
  name                = var.container_registry_name
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "Standard"
  admin_enabled       = true
}

# Container App Environment
resource "azurerm_container_app_environment" "main" {
  name                       = "env-${var.container_app_name}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id
}

# Azure Container App
resource "azurerm_container_app" "main" {
  name                         = var.container_app_name
  container_app_environment_id = azurerm_container_app_environment.main.id
  resource_group_name          = azurerm_resource_group.main.name
  revision_mode                = "Single"

  template {
    container {
      name   = var.container_app_name
      image  = "${azurerm_container_registry.acr.login_server}/${var.container_image_name}:latest"
      cpu    = 0.5
      memory = "1Gi"
      
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = var.app_environment
      }
      
      env {
        name  = "UseHttps"
        value = "false"
      }
    }
    
    min_replicas = 1
    max_replicas = 10
  }
  
  ingress {
    external_enabled = true
    target_port      = 80
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }

  registry {
    server               = azurerm_container_registry.acr.login_server
    username             = azurerm_container_registry.acr.admin_username
    password_secret_name = "registry-password"
  }

  secret {
    name  = "registry-password"
    value = azurerm_container_registry.acr.admin_password
  }
}
