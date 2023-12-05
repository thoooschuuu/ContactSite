resource "azurerm_service_plan" "website_plan" {
  resource_group_name = azurerm_resource_group.rg.name
  name                = format(local.resource_type_templates.app_service_plan, "website")
  location            = local.location
  os_type             = "Linux"
  sku_name            = "B1"
  tags                = local.resource_tags
}

resource "azurerm_linux_web_app" "website_app" {
  resource_group_name = azurerm_resource_group.rg.name
  name                = format(local.resource_type_templates.app_service_environment, "website")
  location            = local.location
  service_plan_id     = azurerm_service_plan.website_plan.id
  https_only          = true

  site_config {
    ftps_state        = "Disabled"
    health_check_path = "/health"
    http2_enabled     = true
    use_32_bit_worker = false

    application_stack {
      dotnet_version = "8.0"
    }
  }

  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    WEBSITE_RUN_FROM_PACKAGE                    = "1"
    ASPNETCORE_ENVIRONMENT                      = "Production"
    APPLICATIONINSIGHTS_CONNECTION_STRING       = azurerm_application_insights.appi.connection_string
    WEBSITE_WEBDEPLOY_USE_SCM                   = "true"
    ApplicationInsightsAgent_EXTENSION_VERSION  = "~3"
    XDT_MicrosoftApplicationInsights_Mode       = "recommended"
    XDT_MicrosoftApplicationInsights_PreemptSdk = "1"
    ConnectionStrings__AppConfig                = azurerm_app_configuration.config.endpoint
  }
}

# plain domain with managed certificate
resource "azurerm_app_service_custom_hostname_binding" "website_plain" {
  hostname            = "thomas-schulze-it-solutions.de"
  app_service_name    = azurerm_linux_web_app.website_app.name
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_app_service_managed_certificate" "website_plain" {
  custom_hostname_binding_id = azurerm_app_service_custom_hostname_binding.website_plain.id
}

resource "azurerm_app_service_certificate_binding" "website_plain" {
  hostname_binding_id = azurerm_app_service_custom_hostname_binding.website_plain.id
  certificate_id      = azurerm_app_service_managed_certificate.website_plain.id
  ssl_state           = "SniEnabled"
}

# www domain with managed certificate
resource "azurerm_app_service_custom_hostname_binding" "website_www" {
  hostname            = "www.thomas-schulze-it-solutions.de"
  app_service_name    = azurerm_linux_web_app.website_app.name
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_app_service_managed_certificate" "website_www" {
  custom_hostname_binding_id = azurerm_app_service_custom_hostname_binding.website_www.id
}

resource "azurerm_app_service_certificate_binding" "website_wwww" {
  hostname_binding_id = azurerm_app_service_custom_hostname_binding.website_www.id
  certificate_id      = azurerm_app_service_managed_certificate.website_www.id
  ssl_state           = "SniEnabled"
}

# roles

resource "azurerm_role_assignment" "website_app_key_vault_reader" {
  scope                = azurerm_key_vault.keyvault.id
  role_definition_name = "Key Vault Reader"
  principal_id         = azurerm_linux_web_app.website_app.identity[0].principal_id
}

resource "azurerm_role_assignment" "website_app_key_vault_secrets_user" {
  scope                = azurerm_key_vault.keyvault.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_linux_web_app.website_app.identity[0].principal_id
}

resource "azurerm_role_assignment" "website_app_app_config_reader" {
  scope                = azurerm_app_configuration.config.id
  role_definition_name = "App Configuration Data Reader"
  principal_id         = azurerm_linux_web_app.website_app.identity[0].principal_id
}

# configuration

resource "azurerm_app_configuration_key" "projects_database_connection_string" {
  configuration_store_id = azurerm_app_configuration.config.id
  key                    = "ProjectsDatabase:ConnectionString"
  type                   = "vault"
  label                  = "contact-site"
  vault_key_reference    = azurerm_key_vault_secret.projects_database_connection_string.id

  depends_on = [
    azurerm_role_assignment.deployment_account_data_owner
  ]
}

resource "azurerm_app_configuration_key" "projects_database_collection_name" {
  configuration_store_id = azurerm_app_configuration.config.id
  key                    = "ProjectsDatabase:DatabaseName"
  label                  = "contact-site"
  value                  = azurerm_cosmosdb_mongo_database.db.name

  depends_on = [
    azurerm_role_assignment.deployment_account_data_owner
  ]
}