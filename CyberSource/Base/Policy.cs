//using CyberSource.WSSecurity;
//using Microsoft.Web.Services3;
//using Microsoft.Web.Services3.Design;
//using Microsoft.Web.Services3.Security;
//using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace CyberSource.Base
{
    //class CybsClientOutputFilter : SoapFilter
    //{
    //    private const string REQUEST_MESSAGE = "requestMessage";

    //    private string keyFullPath;
    //    private string password;
    //    private Logger logger;
    //    private CybsPolicy.RequestType requestType;
    //    private string nspace;
    //    private bool demo;

    //    public CybsClientOutputFilter(CybsSecurityAssertion parentAssertion)
    //    {
    //        keyFullPath = System.IO.Path.Combine(
    //                        parentAssertion.KeysDir,
    //                        parentAssertion.KeyFilename );
    //        password = parentAssertion.Password;
    //        logger = parentAssertion.Logger;
    //        requestType = parentAssertion.RequestType;
    //        nspace = parentAssertion.NamespaceURI;
    //        demo = parentAssertion.Demo;
    //    }

    //    public override SoapFilterResult ProcessMessage(
    //        SoapEnvelope envelope)
    //    {
    //        if (logger != null &&
    //            requestType == CybsPolicy.RequestType.SOAP)
    //        {
    //            XmlNodeList nodes
    //                = envelope.GetElementsByTagName(
    //                    REQUEST_MESSAGE, nspace);
    //            if (nodes != null && nodes.Count > 0)
    //            {
    //                logger.LogRequest( nodes[0], demo );
    //            }
    //        }

    //        //CybsPolicy.SignDocument(envelope, keyFullPath, password);
    //        return( SoapFilterResult.Continue );
    //    }
    //}

    //class CybsClientInputFilter : SoapFilter
    //{
    //    private const string REPLY_MESSAGE = "replyMessage";
    //    private const string NVP_REPLY = "nvpReply";

    //    private Logger logger;
    //    private CybsPolicy.RequestType requestType;
    //    private string nspace;
    //    private bool demo;

    //    public CybsClientInputFilter(CybsSecurityAssertion parentAssertion)
    //    {
    //        logger = parentAssertion.Logger;
    //        requestType = parentAssertion.RequestType;
    //        nspace = parentAssertion.NamespaceURI;
    //        demo = parentAssertion.Demo;
    //    }

    //    public override SoapFilterResult ProcessMessage(
    //        SoapEnvelope envelope)
    //    {
    //        if (logger != null &&
    //            requestType == CybsPolicy.RequestType.SOAP)
    //        {
    //            XmlNodeList nodes
    //                = envelope.GetElementsByTagName(REPLY_MESSAGE, nspace);
    //            if (nodes != null && nodes.Count > 0)
    //            {
    //                logger.LogReply( nodes[0], demo );
    //            }
    //        }
    //        return (SoapFilterResult.Continue);
    //    }
    //}

    //class CybsSecurityAssertion : SecurityPolicyAssertion
    //{
    //    private string keysDir;
    //    private string keyFilename;
    //    private string password;
    //    private Logger logger;
    //    private CybsPolicy.RequestType requestType;
    //    private string nspace;
    //    private bool demo;

    //    public CybsSecurityAssertion(
    //           String keysDir, String keyFilename, String password,
    //           Logger logger, CybsPolicy.RequestType requestType,
    //           string nspace, bool demo )
    //    {
    //        this.keysDir = keysDir;
    //        this.keyFilename = keyFilename;
    //        this.password = password;
    //        this.logger = logger;
    //        this.requestType = requestType;
    //        this.nspace = nspace;
    //        this.demo = demo;
    //    }

    //    public string KeysDir
    //    {
    //        get { return (keysDir); }
    //    }

    //    public string KeyFilename
    //    {
    //        get { return (keyFilename); }
    //    }

    //    public string Password
    //    {
    //        get { return (password); }
    //    }

    //    public Logger Logger
    //    {
    //        get { return (logger); }
    //    }

    //    public CybsPolicy.RequestType RequestType
    //    {
    //        get { return (requestType); }
    //    }

    //    public string NamespaceURI
    //    {
    //        get { return (nspace); }
    //    }

    //    public bool Demo
    //    {
    //        get { return (demo); }
    //    }

    //    public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
    //    {
    //        return (new CybsClientOutputFilter(this));
    //    }

    //    public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
    //    {
    //        if (logger != null &&
    //            requestType == CybsPolicy.RequestType.SOAP)
    //        {
    //            return (new CybsClientInputFilter(this));
    //        }
    //        return (null);
    //    }

    //    public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
    //    {
    //        // we override this method just to satisfy the compiler.
    //        return null;
    //    }

    //    public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
    //    {
    //        // we override this method just to satisfy the compiler.
    //        return null;
    //    }
    //}

    public class CybsPolicy 
    {
        public enum RequestType
        {
            NVP, SOAP
        }

        public CybsPolicy(
            String keysDir, String keyFilename, String password,
            Logger logger, RequestType type, string nspace, bool demo)
        {

        }

        public static String SecurityLibraryVersion {
            get
            {
                return (".Net 1.0.0");
            }
        }

        
    }
}