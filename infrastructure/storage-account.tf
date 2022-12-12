resource "azurerm_storage_account" "contactsite_store" {
  name                     = format(local.storage_name_template, local.resource_type_abbreviations.storage_account)
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = local.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.resource_tags
}
  