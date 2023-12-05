resource "azurerm_service_plan" "resize_function_plan" {
  name                = format(local.resource_type_templates.app_service_plan, "resize")
  location            = local.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "Y1"
  tags                = local.resource_tags
}

resource "azurerm_linux_function_app" "resize_function" {
  name                = format(local.resource_type_templates.function_app, "resize")
  resource_group_name = azurerm_resource_group.rg.name
  location            = local.location

  storage_account_name          = azurerm_storage_account.contactsite_store.name
  storage_uses_managed_identity = true
  service_plan_id               = azurerm_service_plan.resize_function_plan.id

  site_config {
    use_32_bit_worker                      = false
    app_scale_limit                        = 1
    application_insights_connection_string = azurerm_application_insights.appi.connection_string
    minimum_tls_version                    = 1.2

    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_role_assignment" "resize_function_storage_access" {
  scope                = azurerm_storage_account.contactsite_store.id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_linux_function_app.resize_function.identity[0].principal_id
}