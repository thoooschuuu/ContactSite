terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.34.0"
    }
  }

  required_version = ">= 1.1.0"
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = format(local.resource_name_template, local.resource_type_abbreviations["resource_group"])
  location = local.location
  tags     = local.resource_tags
}
