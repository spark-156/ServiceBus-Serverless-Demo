

$manageConnectionString = az servicebus namespace authorization-rule keys list --name RootManageSharedAccessKey --query primaryConnectionString --resource-group nservicebusfunctions --namespace dibrantestsb -o tsv
asb-transport endpoint subscribe RetailDemo.Sales DataModel.Commands.PlaceOrderV1 -c $manageConnectionString
asb-transport endpoint subscribe RetailDemo.Sales DataModel.Commands.PlaceOrderV2 -c $manageConnectionString