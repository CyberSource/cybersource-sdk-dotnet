using System;
using System.Net;
using System.Security.Cryptography;
using System.Xml;
using CyberSource.Clients;


namespace CyberSource.Samples
{
	class XmlSample
	{
		static void Main(string[] args)
		{
			XmlDocument request = new XmlDocument();
			request.Load( args.Length == 1 ? args[0] : "sample.xml" );
            string cybsNamespace = XmlClient.GetRequestNamespace(request);
            if (cybsNamespace == null)
            {
                throw new ApplicationException(
                    "XML document is missing requestMessage");
            }

			try
			{
                Console.WriteLine("starting");
				XmlDocument reply = XmlClient.RunTransaction(request);
				SaveOrderState();
				ProcessReply( reply, cybsNamespace );
			}
			catch (FaultException fe)
			{
                //A custom exception created by the Cybersource Client from 
				SaveOrderState();
				HandleFaultException( fe, cybsNamespace );
			}
            catch (CryptographicException ce)
            {
                SaveOrderState();
                HandleCryptoException(ce);
            }
			catch (WebException we)
			{
				SaveOrderState();
				HandleWebException( we );
			}
            catch (Exception e)
            {
                SaveOrderState();
                HandleException(e);
            }
		}

		private static void SaveOrderState()
		{
			/*
			 * This is where you store the order state in your system for
			 * post-transaction analysis.  Information to store include the
			 * invoice, the values of the reply fields, or the details of the
			 * exception that occurred, if any.
			 */
		}

		private static void ProcessReply(
            XmlDocument reply, string cybsNamespace )
		{
			// Since the CyberSource XML schema uses a non-empty default
			// namespace, XmlNode.SelectSingleNode() will only work if we pass
			// in an XmlNamespaceManager object and an XPath expression that
			// uses a prefix that is defined in said XmlNamespaceManager
			// object.  See documentation on XmlNode.SelectSingleNode() for
			// more details.  Here, we associate an arbitrary prefix "cybs" to
			// the CyberSource namespace used in the input.  We must then use
			// this same prefix in the XPath expressions that we pass to the
			// calls to XmlNode.SelectSingleNode() below.
			XmlNamespaceManager nsmgr 
				= new XmlNamespaceManager( reply.NameTable );
			nsmgr.AddNamespace( "cybs", cybsNamespace );

			XmlNode replyMessage 
				= reply.SelectSingleNode( "cybs:replyMessage", nsmgr );

			string decision = replyMessage.SelectSingleNode(
								"cybs:decision/text()", nsmgr ).Value;

			string template = GetTemplate( decision.ToUpper() );
			string content  = GetContent( replyMessage, nsmgr );

			/*
			 * Display result of transaction.  Being a console application,
			 * this sample simply prints out some text on the screen.  Use
			 * what is appropriate for your system (e.g. ASP.NET pages).
			 */
			Console.WriteLine( template, content );
		}

		private static string GetTemplate( string decision )
		{
			/*
			 * This is where you retrieve the HTML template that corresponds
			 * to the decision.  This template has 'boiler-plate' wording and
			 * can be stored in files or a database.  This is just one way to
			 * retrieve feedback pages.  Use what is appropriate for your
			 * system (e.g. ASP.NET pages).
			 */

			if ("ACCEPT".Equals( decision ))
			{
				return( "The transaction succeeded.{0}" );
			}
			
			if ("REJECT".Equals( decision ))
			{
				return( "Your order was not approved.{0}" );
			}

			// ERROR
			return(
				"Your order could not be completed at this time.{0}" +
				"\nPlease try again later." );
		}

		private static string GetContent(
			XmlNode replyMessage, XmlNamespaceManager nsmgr )
		{
			/*
			 * This is where you retrieve the content that will be plugged
			 * into the template.
			 * 
			 * The strings returned in this sample are mostly to demonstrate
			 * how to retrieve the reply fields.  Your application should
			 * display user-friendly messages.
			 */

			string textVal = replyMessage.SelectSingleNode(
								"cybs:reasonCode/text()", nsmgr ).Value;
			int reasonCode = int.Parse(	textVal );
			switch (reasonCode)
			{
					// Success
				case 100:
					System.Text.StringBuilder sb 
						= new System.Text.StringBuilder();

					sb.Append(
						"\nRequestID: " + 
						replyMessage.SelectSingleNode(
							"cybs:requestID/text()", nsmgr ).Value +
						"\nAuthorization Code: " +
						replyMessage.SelectSingleNode(
							"cybs:ccAuthReply/cybs:authorizationCode/text()",
							nsmgr ).Value );

					XmlNode captureNode = replyMessage.SelectSingleNode(
						"cybs:ccCaptureReply", nsmgr );

					sb.Append(
						"\nCapture Request Time: " +
						captureNode.SelectSingleNode(
							"cybs:requestDateTime/text()", nsmgr ).Value +
						"\nCaptured Amount: " +
						captureNode.SelectSingleNode(
							"cybs:amount/text()", nsmgr ).Value );

					return( sb.ToString() );

					// Missing field(s)
				case 101:
					return(
						"\nThe following required field(s) are missing: " +
						EnumerateValues(
							replyMessage.SelectNodes(
								"cybs:missingField/text()", nsmgr ) ) );

					// Invalid field(s)
				case 102:
					return(
						"\nThe following field(s) are invalid: " +
						EnumerateValues(
							replyMessage.SelectNodes(
								"cybs:invalidField/text()", nsmgr ) ) );

					// Insufficient funds
				case 204:
					return(
						"\nInsufficient funds in the account.  Please use a " +
						"different card or select another form of payment." );

				// add additional reason codes here that you need to handle
				// specifically.

				default:
					// For all other reason codes, return an empty string,
					// in which case, the template will be displayed with no
					// specific content.
					return( String.Empty );
			}	
		}



        private static void HandleException(Exception e)
        {
            string template = GetTemplate("ERROR");

            /*
             * The string returned in this sample is mostly to demonstrate
             * how to retrieve the exception properties.  Your application
             * should display user-friendly messages.
             */
            string content = String.Format(
                "\nAn Exception was returned with Message: '{0}'\n and Stack Trace:" +
                "'{1}'.", e.Message, e.StackTrace);

            Console.WriteLine(template, content);
        }

        private static void HandleCryptoException(CryptographicException e)
        {
            string template = GetTemplate("ERROR");

            /*
             * The string returned in this sample is mostly to demonstrate
             * how to retrieve the exception properties.  Your application
             * should display user-friendly messages.
             */
            
            string content = String.Format(
                "\nA Cryptographic error occurred. Check to make sure that you have a certificate (.p12) file at the location supplied in the configuration file.  Error Message:'{0}'\n\nStack Trace:" +
                "'{1}'.", e.Message, e.StackTrace);

            Console.WriteLine(template, content);
        }

		private static void HandleFaultException(
            FaultException fe, string cybsNamespace )
		{
			string template = GetTemplate( "ERROR" );

			/*
			 * The string returned in this sample is mostly to demonstrate
			 * how to retrieve the exception properties.  Your application
			 * should display user-friendly messages.
			 */
			string content = String.Format( 
				"\nA fault exception was returned with fault code " +
				"'{0}' and message '{1}'.", fe.Code, fe.Message );

			Console.WriteLine( template, content );

			if (fe.Code.Namespace.Equals( cybsNamespace ) &&
				fe.Code.Name.Equals( "CriticalServerError" ))
			{
				/*
				 * The transaction may have been completed by CyberSource.
				 * If your request included a payment service, you should
				 * notify the appropriate department in your company (e.g. by
				 * sending an email) so that they can confirm if the request
				 * did in fact complete by searching the CyberSource Support
				 * Screens using the request id.
				 * 
				 * The line below demonstrates how to retrieve the request id.
				 */

				Console.WriteLine( fe.RequestID );
			}
		}

		private static void HandleWebException( WebException we )
		{
			string template = GetTemplate( "ERROR" );

			/*
			 * The string returned in this sample is mostly to demonstrate
			 * how to retrieve the exception properties.  Your application
			 * should display user-friendly messages.
			 */
			string content = String.Format( 
				"\nFailed to get a response with status '{0}' and " +
				"message '{1}'", we.Status, we.Message );

			Console.WriteLine( template, content );

			if (IsCriticalError( we ))
			{
				/*
				 * The transaction may have been completed by CyberSource.
				 * If your request included a payment service, you should
				 * notify the appropriate department in your company (e.g. by
				 * sending an email) so that they can confirm if the request
				 * did in fact complete by searching the CyberSource Support
				 * Screens using the value of the merchantReferenceCode in
				 * your request.
				 */
			}
		}

		private static string EnumerateValues( XmlNodeList nodes )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			foreach (XmlNode node in nodes)
			{
				sb.Append( node.Value + "\n" );
			}

			return( sb.ToString() );
		}

		private static bool IsCriticalError( WebException we )
		{
			switch (we.Status)
			{
				case WebExceptionStatus.ProtocolError:
					if (we.Response != null)
					{
						HttpWebResponse response 
							= (HttpWebResponse) we.Response;

						// GatewayTimeout may be returned if you are
						// connecting through a proxy server.
						return(	response.StatusCode ==
							HttpStatusCode.GatewayTimeout );
					
					}

					// In case of ProtocolError, the Response property
					// should always be present.  In the unlikely case 
					// that it is not, we assume something went wrong
					// along the way and to be safe, treat it as a
					// critical error.
					return( true );

				case WebExceptionStatus.ConnectFailure:
				case WebExceptionStatus.NameResolutionFailure:
				case WebExceptionStatus.ProxyNameResolutionFailure:
				case WebExceptionStatus.SendFailure:
					return( false );

				default:
					return( true );
			}
		}
	}
}
