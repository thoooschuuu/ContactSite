resource "azurerm_storage_account" "contactsite_store" {
  name                     = format(local.resource_type_templates.storage_account, "main")
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = local.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.resource_tags
}
  