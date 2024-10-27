resource "azurerm_cosmosdb_account" "db_account" {
  name                             = format(local.resource_type_templates.cosmosdb, "account")
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  offer_type                       = "Standard"
  kind                             = "MongoDB"
  mongo_server_version             = "4.2"
  free_tier_enabled                = true
  automatic_failover_enabled       = false
  multiple_write_locations_enabled = false

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = local.location
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_mongo_database" "db" {
  name                = format(local.resource_type_templates.cosmosdb_mongo_db, "db")
  resource_group_name = azurerm_resource_group.rg.name
  account_name        = azurerm_cosmosdb_account.db_account.name
  throughput          = 400
}

# Configurations

resource "azurerm_key_vault_secret" "projects_database_connection_string" {
  name         = "ProjectsDatabase-ConnectionString"
  value        = azurerm_cosmosdb_account.db_account.primary_mongodb_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id

  depends_on = [
    azurerm_role_assignment.deployment_account_data_owner
  ]
}