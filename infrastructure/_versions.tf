terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.83.0"
    }
  }

  required_version = ">= 1.6.4"
}

provider "azurerm" {
  features {}
}