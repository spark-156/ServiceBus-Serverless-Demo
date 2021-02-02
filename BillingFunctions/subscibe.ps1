

$manageConnectionString = az servicebus namespace authorization-rule keys list --name RootManageSharedAccessKey --query primaryConnectionString --resource-group nservicebusfunctions --namespace dibrantestsb -o tsv
asb-transport endpoint subscribe RetailDemo.Billing DataModel.Events.OrderPlaced -c $manageConnectionString
