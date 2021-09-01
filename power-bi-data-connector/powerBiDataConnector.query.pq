// Use this file to write queries to test your data connector
let
     //result = powerBiDataConnector.Contents("https://localhost:5001/api/order")
result = powerBiDataConnector.Contents("https://web-api-order.azurewebsites.net/api/order")
in
    result


//    \\Documents\Microsoft Power BI Desktop\Custom Connectors