# app configuration store
resource "azurerm_app_configuration" "config" {
  name                = format(local.resource_type_templates.app_configuration_store, "main")
  resource_group_name = azurerm_resource_group.rg.name
  location            = local.location
  tags                = local.resource_tags
  sku                 = "free"
}

resource "azurerm_role_assignment" "deployment_account_data_owner" {
  scope                = azurerm_app_configuration.config.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azuread_service_principal.deployment_account.object_id
}

resource "azurerm_role_assignment" "site_group_data_owner" {
  scope                = azurerm_app_configuration.config.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azuread_group.owner.object_id
}

# key vault

resource "azurerm_key_vault" "keyvault" {
  name                     = format(local.resource_type_templates.key_vault, "main")
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = local.location
  tags                     = local.resource_tags
  sku_name                 = "standard"
  tenant_id                = data.azurerm_client_config.current.tenant_id
  purge_protection_enabled = false

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "Create",
      "Get",
    ]

    secret_permissions = [
      "Set",
      "Get",
      "Delete",
      "Purge",
      "Recover"
    ]
  }
}

resource "azurerm_role_assignment" "deployment_account_keyvault_reader" {
  scope                = azurerm_key_vault.keyvault.id
  role_definition_name = "Key Vault Reader"
  principal_id         = data.azuread_service_principal.deployment_account.object_id
}