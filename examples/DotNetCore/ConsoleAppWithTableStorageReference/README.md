# Console Application with Table Storage Reference

This is a guide to using the example project `ConsoleApplicationWithTableStorageReference`. Please ensure that you have completed the following instructions before running the application.

## Create a storage table

Follow the guide for [creating a table in the Azure portal](https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-quickstart-portal).

## Add data to your storage table

Add products to your table by using [these instructions](https://learn.microsoft.com/en-us/azure/cosmos-db/table/quickstart-dotnet?toc=https%3A%2F%2Flearn.microsoft.com%2Fen-us%2Fazure%2Fstorage%2Ftables%2Ftoc.json&bc=https%3A%2F%2Flearn.microsoft.com%2Fen-us%2Fazure%2Fbread%2Ftoc.json&tabs=azure-cli%2Cwindows#create-an-item) and referencing the `Product` record in `Product.cs`.

## Create Azure App Configuration key-values

Add the following key-values to your Azure App Configuration store. `MyShop:Inventory` is a reference to the table you created, and `MyShop:DisplayedProperties` is a list of the columns you wish to include in the final output when printing your table to the console.

1. `MyShop:Inventory`
    - Example value: `https://{account_name}.table.core.windows.net/{table_name}` 
        - `account_name` is the name of your Storage account
        - `table_name` is the name of the table in your Storage account
    - Content-type: `application/x.example.tablereference.product`
2. `MyShop:DisplayedProperties`
    - Example value: `[Name, Quantity]`
    - Content-type: `application/json`

## Run the code

This application maps the key-value `MyShop:Inventory` to a string representation of all the products in your table, displaying the properties specified in `MyShop:DisplayedProperties` for each product. This is then output to the console.

To run the app, use a terminal to navigate to the application directory and run the application.

```dotnetcli
dotnet run
```

The output of the app should be similar to this example, depending on the products added to the table:

```output
Product table:

Name = Ocean Surfboard
Quantity = 8

Name = Sand Surfboard
Quantity = 5
```