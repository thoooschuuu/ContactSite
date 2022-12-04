locals {
  # project settingds
  location         = "westeurope"
  project          = "contactsite"
  environment_name = "prod"

  resource_tags = {
    environment = local.environment_name
    project     = local.project
    costobject  = "marketing"
  }

  # templates

  # example: rg-contactsite-prod-westeurope
  resource_name_template = "%s-${local.project}-${local.environment_name}-${local.location}"
  # example: st1contactsite1prod1westeurope
  storage_name_template = "%s1${local.project}1${local.environment_name}1${local.location}"

  # list of abbreviations for resource types
  resource_type_abbreviations = {
    resource_group          = "rg"
    storage_account         = "st"
    app_service_plan        = "asp"
    app_service_environment = "ase"
    application_insights    = "appi"
    log_analytics_workspace = "log"
  }
}
