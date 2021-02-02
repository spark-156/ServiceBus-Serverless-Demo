# Install asb-transport tool
dotnet tool install -g NServiceBus.Transport.AzureServiceBus.CommandLine

# Select az account
az account set --subscription "Visual Studio Enterprise"

$resourcegroup = 'dibran-nservicebusfunctions-demo'
$servicebusnamespace = 'dibran-servicebus'
$storageaccount = 'dibranstorageaccount'

$billingendpoint = 'RetailDemo.Billing'
$salesendpoint = 'RetailDemo.Sales'
$shippingendpoint = 'RetailDemo.Shipping'

# Create an Azure Service Bus
az servicebus namespace create --name $servicebusnamespace --resource-group $resourcegroup

# Get the connection string
$manageConnectionString = az servicebus namespace authorization-rule keys list --name RootManageSharedAccessKey --query primaryConnectionString --resource-group $resourcegroup --namespace $servicebusnamespace -o tsv

# Create NServiceBus Endpoints and subscribe to messages.
asb-transport queue create error -c $manageConnectionString
asb-transport queue create audit -c $manageConnectionString

asb-transport endpoint create $billingendpoint -c $manageConnectionString

asb-transport endpoint create $billingendpoint -c $manageConnectionString
asb-transport endpoint subscribe $billingendpoint DataModel.Events.OrderPlaced -c $manageConnectionString

asb-transport endpoint create $salesendpoint -c $manageConnectionString
asb-transport endpoint subscribe $salesendpoint DataModel.Commands.PlaceOrderV1 -c $manageConnectionString
asb-transport endpoint subscribe $salesendpoint DataModel.Commands.PlaceOrderV2 -c $manageConnectionString

asb-transport endpoint create $shippingendpoint -c $manageConnectionString
asb-transport endpoint subscribe $shippingendpoint DataModel.Commands.ShipOrder -c $manageConnectionString
asb-transport endpoint subscribe $shippingendpoint DataModel.Events.OrderPlaced -c $manageConnectionString
asb-transport endpoint subscribe $shippingendpoint DataModel.Events.OrderBilled -c $manageConnectionString

# Create a storage account
az storage account create --name $storageaccount --resource-group $resourcegroup