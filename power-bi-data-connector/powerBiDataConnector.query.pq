﻿// Use this file to write queries to test your data connector
let 
    result = powerBiDataConnector.Contents("https://localhost:5001/api/Order")
in
    result
