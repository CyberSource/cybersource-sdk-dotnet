cybersource-sdk-dotnet
======================

.Net SDK for the CyberSource Simple Order API

Moojjoo - Cybersoure is switching over on June 30, 2016 their API to utilize Akamai Content Deliver Network (CDN) - https://support.cybersource.com/cybskb/index?page=content&id=C1683&actp=LIST

Also you can download and review for yourself form my GitHub Branch - https://github.com/moojjoo/cybersource-sdk-dotnet/tree/AkamaiSureRoute

Moojjoo Nuget package - https://www.nuget.org/packages/UnofficialCyberSourceByMoojjoo/1.0.0

##Package 
To install the cybersource-sdk-dotnet from package,Run the following command in the 'NuGet Package Manager Console'.

     PM> Install-Package CyberSource

##Requirements

.NET 4.0 or later

Visual Studio 2012 or later

##Prerequisites


A CyberSource Evaluation account. Sign up here:  <http://www.cybersource.com/register>

* Complete your Evaluation account creation by following the instructions in the Registration email

Transaction Security Keys

* Create security keys in the Enterprise Business Center (ebctest) after you've created your Merchant Admin account. 
Refer to our Developer's Guide for details <http://apps.cybersource.com/library/documentation/dev_guides/security_keys/creating_and_using_security_keys.pdf> under Simple Order API Security Keys to generate .P12 key.

##Installing the SDK 

1. Download the cybersource-sdk-dotnet-master.zip package into a directory of your choice. 

2. Extract and go to the cybersource-sdk-dotnet directory.

3. Open Solution CyberSource from in Visual Studio.

4. Build/Rebuild the Solution.

##Running the Samples

1. Copy samples.xml from cybersource-sdk-dotnet directory to cybersource-sdk-dotnet\CyberSourceSamples\bin

2. Update following properties in XmlSample.exe.config (remember to update app.config to keep these values when rebuilding).
	
	a. cybs.merchantID
	
	b. cybs.keysDirectory
	
	c. cybs.logDirectory
	
	d. cybs.proxyURL

3. Run XmlSample.exe.

## Working with other versions of Cybersource API

The version of the CyberSource Web Services API supported by the clients is indicated at the top of this README.  
To be able to communicate with a other versions:

* If you are using the Name-Value Pair (NVP) or the SOAP client,

    1. Load src\CyberSource.Clients.sln in Visual Studio.

    2.  
        a. Find the “Service References” folder in the solution Explorer
	
        b. Right click on “NVPServiceReference” and select “Configure Service Reference”
	
        c. Update the “Address” field with New WSDL URL. You should normally only need to update the version number at the end.
	
        d. Repeat for steps B and C for “SoapServiceReference” (also in the Service References folder)

    3. Build the Release configuration.

    4. Save a copy of the original CyberSource.Clients.dll and replace it with the one just built.


* If you are using the XML client,

    There is no need to update the client. Simply start using the new namespace URI in your input XML documents.
    The client will automatically pick it up and use the specified version.

## SUPPORT FOR MULTIPLE MERCHANTS

Except for cybs.merchantID and the cybs.proxy* settings, all other config setting keys may be prefixed with the merchant id to tell the client that it is only applicable to that merchant id.  
Keys without any merchant prefix will be used in the absence of the corresponding merchant-specific one.

For example:

	<add key="cybs.merchant123.sendToProduction" value="false"/>
	<add key="cybs.sendToProduction" value="true"/>

All requests for merchant123 will go to the test server, all other requests will go to the production server.

Alternatively, you can pre-create and cache a CyberSource.Clients.Configuration object for each merchant and pass the appropriate one to the RunTransaction() method.


##Documentation

For more information about CyberSource services, see <http://www.cybersource.com/developers/documentation>

For all other support needs, see <http://www.cybersource.com/support>
