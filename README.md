cybersource-sdk-dotnet
======================

This is the temporary private repo for the Cybs .Net SDK

======================

##Requirements

.NET 4.0 or later

Visual Studio 2012 or later

##Prerequisites


A CyberSource Evaluation account. Sign up here:  <http://www.cybersource.com/register>

* Complete your Evaluation account creation by following the instructions in the Registration email

Transaction Security Keys

* Create security keys in the Enterprise Business Center (ebctest) after you've created your Merchant Admin account. 
Refer to our Developer's Guide for details <http://www.cybersource.com/developers/integration_methods/simple_order_and_soap_toolkit_api/soap_api/html>

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

##Documentation

For more information about CyberSource services, see <http://www.cybersource.com/developers/documentation>

For all other support needs, see <http://www.cybersource.com/support>
