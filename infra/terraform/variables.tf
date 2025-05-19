variable "subscription_id" {
  description = "Azure Subscription ID"
  type        = string
}

variable "resource_group_name" {
  description = "Name of the Resource Group for Azure resources"
  type        = string
  default     = "rg-tpg-app"
}

variable "location" {
  description = "Location of Azure resources"
  type        = string
  default     = "eastus"
}

variable "container_app_name" {
  description = "Name of the Container App"
  type        = string
  default     = "tpg-app"
}

variable "container_registry_name" {
  description = "Name of the Container Registry"
  type        = string
  default     = "tpgconreg"
}

variable "container_image_name" {
  description = "Name of the container image"
  type        = string
  default     = "tpg-blazor-app"
}

variable "app_environment" {
  description = "Application environment"
  type        = string
  default     = "Production"
}
