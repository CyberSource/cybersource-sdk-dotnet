using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;
using CyberSource.Clients;
using CyberSource.Clients.SoapServiceReference;
using System.Security.Cryptography;

namespace CyberSource.Samples
{
	class SoapSample
	{
		static void Main(string[] args)
		{
			RequestMessage request = new RequestMessage();

            // we will let the client pick up the merchantID
            // from the config file.  In multi-merchant scenarios,
            // you would set a merchantID in each request.

            // this sample requests auth and capture
	
			// Credit Card Authorization
			request.ccAuthService = new CCAuthService();
			request.ccAuthService.run = "true";

			// Credit Card Capture
			request.ccCaptureService = new CCCaptureService();
			request.ccCaptureService.run = "true";


			// add required fields

			request.merchantReferenceCode = "your_merchant_reference_code";

			BillTo billTo = new BillTo();
			billTo.firstName = "John";
			billTo.lastName = "Doe";
			billTo.street1 = "1295 Charleston Road";
			billTo.city = "Mountain View";
			billTo.state = "CA";
			billTo.postalCode = "94043";
			billTo.country = "US";
			billTo.email = "null@cybersource.com";
			billTo.ipAddress = "10.7.111.111";
			request.billTo = billTo;

			Card card = new Card();
			card.accountNumber = "4111111111111111";
			card.expirationMonth = "12";
			card.expirationYear = "2020";
			request.card = card;

			PurchaseTotals purchaseTotals = new PurchaseTotals();
			purchaseTotals.currency = "USD";
			request.purchaseTotals = purchaseTotals;

			// there are two items in this sample
			request.item = new Item[2];

			Item item = new Item();
			item.id = "0";
			item.unitPrice = "12.34";
			request.item[0] = item;

			item = new Item();
			item.id = "1";
			item.unitPrice = "56.78";
			request.item[1] = item;

			// add optional fields here per your business needs

			try
			{
				ReplyMessage reply = SoapClient.RunTransaction( request );
				SaveOrderState();
				ProcessReply( reply );
			}
           /*** There are many specific exceptions that could be caught here. Only a few are provided as examples. 
             This is a Windows Communication Foundation (WCF) Client and uses exceptions from the System.ServiceModel
             Namespaces. Please refer to the Microsoft documentation to better understand what these exceptions mean.
                
            ***/

           //System.ServiceModel Exception examples.
            catch (EndpointNotFoundException e)
            {
                // This is thrown when a remote endpoint could not be found or reached.  
                // The remote endpoint is down, the client network connection is down, 
                // the remote endpoint is unreachable, or because the remote network is unreachable.

                SaveOrderState();
                HandleException(e);
            }
            catch (ChannelTerminatedException e)
            {
                // This is typically thrown on the client when a channel is terminated due to the server closing the connection.
                SaveOrderState();
                HandleException(e);
            }
            //System.ServiceModel.Security Exception example.
            catch (MessageSecurityException e)
            {
                //Represents an exception that occurred when there is something wrong with the security applied on a message. Possibly a bad signature.
                SaveOrderState();
                HandleSecurityException(e);
            }
            //System.Security.Cryptography exception    
            catch (CryptographicException ce)
            {
                //Represents a problem with your X509 certificate (.p12 file) or a problem creating a signature from the certificate.
                SaveOrderState();
                HandleCryptoException(ce);
            }
            //System.Net exception    
            catch (WebException we)
            {
                //The WebException class is thrown by classes descended from WebRequest and WebResponse that implement pluggable protocols for accessing the Internet.
                SaveOrderState();
                HandleWebException(we);
            }
            //Any other exception!
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

		private static void ProcessReply( ReplyMessage reply )
		{
    		string template = GetTemplate( reply.decision.ToUpper() );
			string content  = GetContent( reply );

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

		private static string GetContent( ReplyMessage reply )
		{
			/*
			 * This is where you retrieve the content that will be plugged
			 * into the template.
			 * 
			 * The strings returned in this sample are mostly to demonstrate
			 * how to retrieve the reply fields.  Your application should
			 * display user-friendly messages.
			 */

			int reasonCode = int.Parse( reply.reasonCode );
			switch (reasonCode)
			{
					// Success
				case 100:
					return( 
						"\nRequest ID: " + reply.requestID +
						"\nAuthorization Code: " +
							reply.ccAuthReply.authorizationCode +
						"\nCapture Request Time: " + 
							reply.ccCaptureReply.requestDateTime +
						"\nCaptured Amount: " +
							reply.ccCaptureReply.amount );

					// Missing field(s)
				case 101:
					return(
						"\nThe following required field(s) are missing: " +
						EnumerateValues( reply.missingField ) );

					// Invalid field(s)
				case 102:
					return(
						"\nThe following field(s) are invalid: " +
						EnumerateValues( reply.invalidField ) );

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

        private static void HandleSecurityException(MessageSecurityException e)
        {
            string template = GetTemplate("ERROR");

            /*
             * The string returned in this sample is mostly to demonstrate
             * how to retrieve the exception properties.  Your application
             * should display user-friendly messages.
             */
            string content = String.Format(
                "\nA Security exception was returned with message '{1}'.", e.Message);

            Console.WriteLine(template, content);
        }
        
        private static void HandleCryptoException(Exception e)
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

		private static string EnumerateValues( string[] array )
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (string val in array)
			{
				sb.Append( val + "\n" );
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
