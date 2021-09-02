// This file contains your Data Connector logic
section powerBiDataConnector;

client_id = Text.FromBinary(Extension.Contents("client_id.txt"));
client_secret = Text.FromBinary(Extension.Contents("client_secret.txt"));
redirect_uri = "https://oauth.powerbi.com/views/oauthredirect.html";
token_uri = "https://login.microsoftonline.com/49ae1e1a-3f6e-4708-a119-3db19ac9d323/oauth2/v2.0/token";
authorize_uri = "https://login.microsoftonline.com/49ae1e1a-3f6e-4708-a119-3db19ac9d323/oauth2/v2.0/authorize";
logout_uri = "https://login.microsoftonline.com/logout.srf";


// Login modal window dimensions
windowWidth = 720;
windowHeight = 1024;

// OAuth2 scope
// scope_prefix = "";
scopes =   "https://spseventdackar.onmicrosoft.com/api/orders/mines";//  "order_api.orders";

[DataSource.Kind="powerBiDataConnector", Publish="powerBiDataConnector.Publish"]
shared powerBiDataConnector.Contents = (url as text) =>
    let
       source = Json.Document(Web.Contents(url)),
       #"Converted to Table" = Table.FromList(source, Splitter.SplitByNothing(), null, null, ExtraValues.Error),
       #"Expanded Column1" = Table.ExpandRecordColumn(#"Converted to Table", "Column1", {"orderId", "productName", "unitPrice", "quantity", "customerEmail"}, {"orderId", "productName", "unitPrice", "quantity", "customerEmail"})
    in
       #"Expanded Column1";

// Data Source Kind description
powerBiDataConnector = [
    Authentication = [
        OAuth = [
            StartLogin=StartLogin,
            FinishLogin=FinishLogin,
            Refresh=Refresh,
            Logout=Logout
        ]
    ],
    Label = Extension.LoadString("DataSourceLabel")
];

// Data Source UI publishing description
powerBiDataConnector.Publish = [
    Beta = true,
    Category = "Other",
    ButtonText = { Extension.LoadString("ButtonTitle"), Extension.LoadString("ButtonHelp") },
    LearnMoreUrl = "https://powerbi.microsoft.com/",
    SourceImage = powerBiDataConnector.Icons,
    SourceTypeImage = powerBiDataConnector.Icons
];

// Helper functions for OAuth2: StartLogin, FinishLogin, Refresh, Logout
StartLogin = (resourceUrl, state, display) =>
    let
        authorizeUrl = authorize_uri & "?" & Uri.BuildQueryString([
            response_type = "code",
            client_id = client_id,  
            redirect_uri = redirect_uri,
            state = state,
            scope = scopes
        ])
    in
        [
            LoginUri = authorizeUrl,
            CallbackUri = redirect_uri,
            WindowHeight = 720,
            WindowWidth = 1024,
            Context = null
        ];

FinishLogin = (context, callbackUri, state) =>
    let
        // parse the full callbackUri, and extract the Query string
        parts = Uri.Parts(callbackUri)[Query],
        // if the query string contains an "error" field, raise an error
        // otherwise call TokenMethod to exchange our code for an access_token
        result = if (Record.HasFields(parts, {"error", "error_description"})) then 
                    error Error.Record(parts[error], parts[error_description], parts)
                 else
                    TokenMethod("authorization_code", "code", parts[code])
    in
        result;

Refresh = (resourceUrl, refresh_token) => TokenMethod("refresh_token", "refresh_token", refresh_token);

Logout = (token) => logout_uri;

// see "Exchange code for access token: POST /oauth/token" at https://cloud.ouraring.com/docs/authentication for details
TokenMethod = (grantType, tokenField, code) =>
    let
        queryString = [
            grant_type = "authorization_code",
            redirect_uri = redirect_uri,
            client_id = client_id,
            client_secret = client_secret
        ],
        queryWithCode = Record.AddField(queryString, tokenField, code),

        tokenResponse = Web.Contents(token_uri, [
            Content = Text.ToBinary(Uri.BuildQueryString(queryWithCode)),
            Headers = [
                #"Content-type" = "application/x-www-form-urlencoded",
                #"Accept" = "application/json"
            ],
            ManualStatusHandling = {400} 
        ]),
        body = Json.Document(tokenResponse),
        result = if (Record.HasFields(body, {"error", "error_description"})) then 
                    error Error.Record(body[error], body[error_description], body)
                 else
                    body
    in
        result;



powerBiDataConnector.Icons = [
    Icon16 = { Extension.Contents("powerBiDataConnector16.png"), Extension.Contents("powerBiDataConnector20.png"), Extension.Contents("powerBiDataConnector24.png"), Extension.Contents("powerBiDataConnector32.png") },
    Icon32 = { Extension.Contents("powerBiDataConnector32.png"), Extension.Contents("powerBiDataConnector40.png"), Extension.Contents("powerBiDataConnector48.png"), Extension.Contents("powerBiDataConnector64.png") }
];
