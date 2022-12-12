resource "azurerm_resource_group" "rg" {
  name     = format(local.resource_type_templates.resource_group, "main")
  location = local.location
  tags     = local.resource_tags
}
