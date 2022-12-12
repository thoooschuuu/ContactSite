locals {
  retention_in_days = 30
}

resource "azurerm_log_analytics_workspace" "log" {
  name                = format(local.resource_name_template, local.resource_type_abbreviations.log_analytics_workspace)
  location            = local.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = local.retention_in_days
  tags                = local.resource_tags
}

resource "azurerm_application_insights" "appi" {
  name                = format(local.resource_name_template, local.resource_type_abbreviations.application_insights)
  location            = local.location
  resource_group_name = azurerm_resource_group.rg.name
  workspace_id        = azurerm_log_analytics_workspace.log.id
  application_type    = "web"
  tags                = local.resource_tags
  retention_in_days   = local.retention_in_days
}