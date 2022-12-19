resource "azurerm_cosmosdb_account" "db_account" {
  name                            = format(local.resource_type_templates.cosmosdb, "account")
  location                        = local.location
  resource_group_name             = azurerm_resource_group.rg.name
  offer_type                      = "Standard"
  kind                            = "GlobalDocumentDB"
  enable_free_tier                = true
  enable_automatic_failover       = false
  enable_multiple_write_locations = false

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = local.location
    failover_priority = 0
  }

  capacity {
    total_throughput_limit = 400
  }
}

resource "azurerm_cosmosdb_sql_database" "db" {
  name                = format(local.resource_type_templates.cosmosdb_no_sql, "db")
  resource_group_name = azurerm_resource_group.rg.name
  account_name        = azurerm_cosmosdb_account.db_account.name
  throughput          = 400
}

resource "azurerm_cosmosdb_sql_container" "container" {
  name                = "Projects"
  resource_group_name = azurerm_resource_group.rg.name
  account_name        = azurerm_cosmosdb_account.db_account.name
  database_name       = azurerm_cosmosdb_sql_database.db.name
  partition_key_path  = "/id"
  throughput          = 400
}