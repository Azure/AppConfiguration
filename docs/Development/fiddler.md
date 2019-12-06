# Fiddler Configuration - REST API Reference
#
To test the REST API using [Fiddler](https://www.telerik.com/fiddler), requests need to include the headers required for [authentication](./authentication/hmac.md). Here's how to configure Fiddler for testing the REST API, generating the authentication headers automatically:

1. Ensure that TLS 1.2 is an allowed protocol

    Open the HTTPS options (Tools->Options->HTTPS). Ensure that `Decrypt HTTPS traffic` is checked.  Then in the list of protocols, add `tls1.2` if not present.

2. Open "Fiddler Script Editor" or press Ctrl-R within Fiddler

3. Add the following code inside the Handlers class before the OnBeforeRequest function

    ```js
    static function SignRequest(oSession: Session, credential: String, secret: String) {
        var utcNow = DateTimeOffset.UtcNow.ToString("r", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        var contentHash = ComputeSHA256Hash(oSession.RequestBody);
        var stringToSign = oSession.RequestMethod.ToUpperInvariant() + "\n" + oSession.PathAndQuery + "\n" + utcNow +";" + oSession.hostname + ";" + contentHash;
        var signature = ComputeHMACHash(secret, stringToSign);
    
        oSession.oRequest.headers["x-ms-date"] = utcNow;
        oSession.oRequest.headers["x-ms-content-sha256"] = contentHash;
        oSession.oRequest.headers["Authorization"] = "HMAC-SHA256 Credential=" + credential + "&SignedHeaders=x-ms-date;host;x-ms-content-sha256&Signature=" + signature;
    }
    
    static function ComputeSHA256Hash(content: Byte[]) {
        var sha256 = System.Security.Cryptography.SHA256.Create();
        try {
            return Convert.ToBase64String(sha256.ComputeHash(content));
        }
        finally {
            sha256.Dispose();
        }
    }
    
    static function ComputeHMACHash(secret: String, content: String) {
        var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(secret));
        try {
            return Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(content)));
        }
        finally {
            hmac.Dispose();
        }
    }
    ```

4. Add the following code at the end of the OnBeforeRequest function and update the access key as indicated by the TODO comment

    ```js
    if (oSession.isFlagSet(SessionFlags.RequestGeneratedByFiddler) &&
        oSession.hostname.EndsWith(".azconfig.io", StringComparison.OrdinalIgnoreCase)) {

        // TODO: Replace the following placeholders with your access key
        var credential = "<Credential>"; // Id
        var secret = "<Secret>"; // Value
    
        SignRequest(oSession, credential, secret);
    }
    ```

5. Use [Fiddler's Composer](https://docs.telerik.com/fiddler/Generate-Traffic/Tasks/CreateNewRequest) to generate and send a request
