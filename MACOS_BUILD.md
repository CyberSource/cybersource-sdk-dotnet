# Building and Testing on macOS

This SDK targets .NET Framework 4.8. Building and running on macOS requires [Mono](https://www.mono-project.com/).

## Limitations

Only `XmlSample` works on Mono. `NVPSample`, `SCMPSample`, and `SoapSample` use WCF bindings
(`SecurityBindingElement.CreateMutualCertificateDuplexBindingElement`) that are not implemented
in Mono's WCF runtime and will throw `NotImplementedException`.

## Prerequisites

Install Mono via Homebrew:

```
brew install mono
```

Sync the macOS root certificates into Mono's trust store (required for TLS — only needed once):

```
cert-sync /etc/ssl/cert.pem
```

## Build

```
xbuild CyberSource.sln /p:Configuration=Debug
```

Output assemblies land in `CyberSourceSamples/bin/`.

## Configure

Edit `CyberSourceSamples/bin/XmlSample.exe.config` and set:

```xml
<add key="cybs.merchantID" value="YOUR_MERCHANT_ID"/>
<add key="cybs.keysDirectory" value="/path/to/directory/containing/p12"/>
<add key="cybs.serverURL" value="YOUR_SERVER_URL"/>
```

The `.p12` file must be named `<merchantID>.p12` and live in `keysDirectory`.

To use MLE (`cybs.useSignAndEncrypted=true`), also set:

```xml
<add key="cybs.useSignAndEncrypted" value="true"/>
```

Mono's `X509Certificate2Collection.Import` only loads the first certificate with a matching
private key from a PKCS12 file, so additional certificates such as `CyberSource_SJC_US` are
not loaded. The SDK falls back to loading it from a separate PEM file.
Extract it from any `.p12` file that contains it:

```
openssl pkcs12 -in <merchantID>.p12 -passin pass:<password> -nokeys 2>/dev/null \
  | awk '/friendlyName:.*CyberSource_SJC_US/{f=1} f&&/BEGIN CERTIFICATE/{p=1} p{print} /END CERTIFICATE/{p=0;f=0}' \
  > CyberSource_SJC_US.pem
```

Place `CyberSource_SJC_US.pem` in the same `keysDirectory` as the `.p12` file and the
SDK will pick it up automatically.

## Run a test request

```
cd CyberSourceSamples/bin
mono XmlSample.exe
```

A successful response looks like:

```
starting
The transaction succeeded.
RequestID: ...
Authorization Code: ...
Capture Request Time: ...
Captured Amount: ...
```

The sample request is defined in `CyberSourceSamples/bin/sample.xml`.
