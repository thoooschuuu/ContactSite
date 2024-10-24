resource "azurerm_resource_group" "rg" {
  name     = format(local.resource_type_templates.resource_group, "main")
  location = local.location
  tags     = local.resource_tags
}

data "azurerm_client_config" "current" {}

data "azuread_group" "owner" {
  display_name = var.aad_group_site_owners
}
