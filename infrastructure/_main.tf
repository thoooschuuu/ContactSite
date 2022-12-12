resource "azurerm_resource_group" "rg" {
  name     = format(local.resource_name_template, local.resource_type_abbreviations.resource_group)
  location = local.location
  tags     = local.resource_tags
}
