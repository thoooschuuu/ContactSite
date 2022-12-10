resource "azurerm_app_service_plan" "plan" {
    resource_group_name = azurerm_resource_group.rg.name
    name                = format(local.resource_name_template, local.resource_type_abbreviations["app_service_plan"])
    location            = local.location
    os_type             = "Linux"
    sku_name            = "B1"
}

resource "azurerm_linux_web_app" "app" {
    resource_group_name = azurerm_resource_group.rg.name
    name                = format(local.resource_name_template, local.resource_type_abbreviations["app_service"])
    location            = local.location
    app_service_plan_id = azurerm_app_service_plan.plan.id
    
    site_config {
        ftps_state = "Disabled"
        health_check_path = "/health"
        http2_enabled = true
        use_32_bit_worker_process = false
        
        application_stack {
            dotnet_version = "7.0"
        }
    }

    identity {
        type = "SystemAssigned"
    }

    app_settings = {
      WEBSITE_RUN_FROM_PACKAGE = "1"
      ASPNETCORE_ENVIRONMENT = "Production"
      APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.appi.connection_string
    }
}