locals {
  # project settingds
  location         = "westeurope"
  location_short   = "weu"
  project          = "contactsite"
  project_short    = "cs"
  environment_name = "prod"

  resource_tags = {
    environment = local.environment_name
    project     = local.project
    costobject  = "marketing"
  }

  # templates

  # example: rg-contactsite-main-prod-westeurope
  resource_name_template = "%s-${local.project}-%s-${local.environment_name}-${local.location}"
  # example: rg-cs-main-prod-weu
  resource_name_template_short = "%s1${local.project_short}1%s1${local.environment_name}1${local.location_short}"
  # example: st1cs1main1prod1weu
  storage_name_template = "%s1${local.project_short}1%s1${local.environment_name}1${local.location_short}"

  # list of abbreviations for resource types
  resource_type_abbreviations = {
    resource_group          = "rg"
    storage_account         = "st"
    app_service_plan        = "asp"
    app_service_environment = "ase"
    function_app            = "func"
    application_insights    = "appi"
    log_analytics_workspace = "log"
    app_configuration_store = "appcs"
    key_vault               = "kv"
    cosmosdb                = "cosmos"
    cosmosdb_no_sql         = "cosno"
    cosmosdb_mongo_db       = "cosmon"
  }

  resource_type_templates = {
    resource_group          = format(local.resource_name_template, local.resource_type_abbreviations.resource_group, "%s")
    storage_account         = format(local.storage_name_template, local.resource_type_abbreviations.storage_account, "%s")
    app_service_plan        = format(local.resource_name_template, local.resource_type_abbreviations.app_service_plan, "%s")
    app_service_environment = format(local.resource_name_template, local.resource_type_abbreviations.app_service_environment, "%s")
    function_app            = format(local.resource_name_template, local.resource_type_abbreviations.function_app, "%s")
    application_insights    = format(local.resource_name_template, local.resource_type_abbreviations.application_insights, "%s")
    log_analytics_workspace = format(local.resource_name_template, local.resource_type_abbreviations.log_analytics_workspace, "%s")
    app_configuration_store = format(local.resource_name_template, local.resource_type_abbreviations.app_configuration_store, "%s")
    key_vault               = format(local.resource_name_template_short, local.resource_type_abbreviations.key_vault, "%s")
    cosmosdb                = format(local.resource_name_template, local.resource_type_abbreviations.cosmosdb, "%s")
    cosmosdb_no_sql         = format(local.resource_name_template, local.resource_type_abbreviations.cosmosdb_no_sql, "%s")
    cosmosdb_mongo_db       = format(local.resource_name_template, local.resource_type_abbreviations.cosmosdb_mongo_db, "%s")
  }
}
