# CyberSource .NET SDK
This is the .NET SDK for the CyberSource Simple Order API

## Package 
To install the `cybersource-sdk-dotnet` from package, run the following command in the 'NuGet Package Manager Console'.
```
PM> Install-Package CyberSource
```
## Requirements
- .NET 4.0 or later
- Visual Studio 2012 or later

## Prerequisites
A CyberSource Evaluation account. Sign up at <http://www.cybersource.com/register>
- Complete your Evaluation account creation by following the instructions in the Registration email.
Transaction Security Keys
- Create security keys in the Enterprise Business Center (ebctest) after you've created your Merchant Admin account. 
Refer to our [Developer Guide](http://apps.cybersource.com/library/documentation/dev_guides/security_keys/creating_and_using_security_keys.pdf) for details on how to generate .P12 key.

## CyberSource Web Services API Version 
NuGet 1.4.3 has v1.164 urn:schemas-cybersource-com:transaction-data-1.164

For latest version, check this https://ics2ws.ic3.com/commerce/1.x/transactionProcessor/

## Installing the SDK 
- Download the `cybersource-sdk-dotnet-master.zip` package into a directory of your choice.
- Extract and go to the cybersource-sdk-dotnet directory.
- Open Solution CyberSource from in Visual Studio.
- Build/Rebuild the Solution.

## Running the Samples
- Copy `samples.xml` from `cybersource-sdk-dotnet` directory to `cybersource-sdk-dotnet\CyberSourceSamples\bin`
- Update following properties in XmlSample.exe.config (remember to update app.config to keep these values when rebuilding).
  - cybs.merchantID
  - cybs.keysDirectory
  - cybs.logDirectory
  - cybs.proxyURL
- Run `XmlSample.exe`.

## Working with other versions of Cybersource API
The version of the CyberSource Web Services API supported by the clients is indicated at the top of this README. To be able to communicate with a other versions:
- If you are using the Name-Value Pair (NVP) or the SOAP client, load `src\CyberSource.Clients.sln` in Visual Studio.
  - Find the "Service References" folder in the solution Explorer
    1. Right click on "NVPServiceReference" and select "Configure Service Reference"
    2. Update the "Address" field with New WSDL URL. You should normally only need to update the version number at the end.
  - Repeat for steps (a) and (b) for "SoapServiceReference" (also in the Service References folder)
  - Build the Release configuration.
  - Save a copy of the original CyberSource.Clients.dll and replace it with the one just built.
- If you are using the XML client, there is no need to update the client. Simply start using the new namespace URI in your input XML documents. The client will automatically pick it up and use the specified version.

## Support for Multiple Merchants
Except for cybs.merchantID and the cybs.proxy* settings, all other config setting keys may be prefixed with the merchant id to tell the client that it is only applicable to that merchant id. Keys without any merchant prefix will be used in the absence of the corresponding merchant-specific one. For example:
```xml
<add key="cybs.merchant123.sendToProduction" value="false"/>
<add key="cybs.sendToProduction" value="true"/>
```
All requests for merchant123 will go to the test server, all other requests will go to the production server.

Alternatively, you can pre-create and cache a CyberSource.Clients.Configuration object for each merchant and pass the appropriate one to the RunTransaction() method.

## Support for Akamai
- Set `sendToAkamai` config parameter with toggle value "true/false" to turn on/off routing requests through Akamai to Cybersource. By default, it is set to true.
- `serverURL` config parameter will take precedence over `sendToProduction` and `sendToAkamai` config parameters. By default the `serverURL` configuration is commented out.

## Support for Message level encryption
- `useSignAndEncrypted` flag introduced to turn encryption "on" or "off".
  - Default value of the flag: FALSE (Only does signing and not encryption).
- Set `useSignAndEncrypted` to true to encrypt the signed Payload. For more information about MLE, See Message Level Encryption details below.

CyberSource supports Message Level Encryption (MLE) for Simple Order API. Message level encryption conforms to the SOAP Security 1.0 specification published by the OASIS standards group. 

## Authentication Details
    Message level encryption authenticates using the same mechanism as signed SOAP messages. The signature creation involves utilizing the merchants private key which combined with a hash of the message to be signed, can be validated with the merchants certificate and the message which was signed. 
    The merchant certificate is included in the SOAP message for both signature and message level encryption. Message level encryption, encrypts a temporary message key for a specific recipient. This is done by encrypting the temporary message key with the recipient’s public certificate. Therefore only the party holding the private key (CyberSource) can decrypt the temporary message key. The merchant sending the request must be a valid merchant for the environment which the message is being processed in. After validating the merchant and retrieving the CyberSource copy of the merchant certificate from our database, these additional authentication steps are performed;
    1. The certificate sent in the message must have valid trust chain with the CyberSource certificate authority as the root signer.
    2. A certificate belonging to the merchant which sent the message must exist within our database, having the exact serial number of the certificate provided. 
    3. Our record of the certificate must have a valid start and end date for the transaction time sent.
    4. Our record of the certificate must have a “active” state (ie. Not deactivated by support).
    5. If merchant is reseller, the merchant must allow reseller to act upon their behalf and reseller must be configured as a reseller and the provided merchant must be configured as a merchant of this reseller. Additionally all above authorizations apply.

## Cryptography Algorithms
    CyberSource utilizes the following algorithms for this implementation. While others may work, the following are validated and recommended. SSL is used for transport security even with encrypted messages. CyberSource asymmetric keys are RSA 2048 keys and therefore your cryptography API should support 2048 bit RSA keys and signatures create with them. The messages are encrypted with a temporary derived key which is used per message. This derived key is AES 256 bit and utilizes CBC blocking mode for encryption. The derived key is encrypted with the recipient ( CyberSource ) public key. The key exchange algorithm used is RSA-OAEP.

## Meta Key support
Meta Key is a key generated by an entity that can be used to authenticate on behalf of other entities provided that the entity which holds key is a parent entity or associated as a partner.

SOAPI SDK supports meta key. However you have set below properties in app.config file.

    cybs.merchantID=  <comment/remove this line> 
    cybs.keysDirectory=<Directory where P12 is present>
    cybs.keyAlias=<Refers to the portfolio>
    cybs.password=<Password of p12>
    cybs.keyFilename= <metakey downloaded from portfolio MID>
 
 Auth sample payload:
  
    merchantID=<meta_2232323> <Refers to the Child transactional MID>
    ccAuthService_run=true
    merchantReferenceCode=MRC-14344 
    billTo_firstName=John

## Documentation
- For more information about CyberSource services, see <http://www.cybersource.com/developers/documentation>
- For all other support needs, see <http://www.cybersource.com/support>
