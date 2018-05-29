using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Security;
using CyberSource.Clients;
using CyberSource.Clients.SoapServiceReference;
using System.Security.Cryptography;
using System.Threading;

namespace CyberSource.Samples
{
    class SoapSample
    {
        enum RunAs
        {
            Synchronously = 1,
            Asynchronously = 2
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to run this sample Synchronously or Asycnhronously?");
            Console.WriteLine("1: Synchronously");
            Console.WriteLine("2: Asynchronously");
            RunAs runAs = (RunAs)Enum.Parse(typeof(RunAs), Console.ReadLine());

            Console.Clear();

            Console.WriteLine("Select the transaction type:\n" +
                "1: Auth \n" +
                "2: Auth Reversal\n" +
                "3: EMV Auth\n" +
                "4: Level2 capture\n" +
                "5: Payment Network Tokenization\n" +
                "6: Refund Request\n" +
                "7: Visa Checkout Auth\n" +
                "8: Android Pay Auth\n" +
                "9: Apple Pay Auth\n" +
                "10: Sale\n"
                );

            SampleTransactions sampleTransaction = new SampleTransactions();
            RequestMessage request = null;

            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    request = sampleTransaction.authRequest();
                    break;

                case 2:
                    request = sampleTransaction.authRevRequest();
                    break;

                case 3:
                    request = sampleTransaction.emvAuthRequest();
                    break;

                case 4:
                    request = sampleTransaction.level2CaptureRequest();
                    break;

                case 5:
                    request = sampleTransaction.paymentNetworkTokenizationRequest();
                    break;

                case 6:
                    request = sampleTransaction.refundRequest();
                    break;

                case 7:
                    request = sampleTransaction.visaCheckoutAuthRequest();
                    break;

                case 8:
                    request = sampleTransaction.androidPayAuthRequest();
                    break;

                case 9:
                    request = sampleTransaction.applePayAuthRequest();
                    break;

                case 10:
                    request = sampleTransaction.saleRequest();
                    break;

            }

            Console.WriteLine($"Running sample {runAs.ToString()}");

            try
            {
                ReplyMessage reply;

                if (runAs == RunAs.Synchronously)
                {
                    reply = SoapClient.RunTransaction(request);
                }
                else
                {
                    //Await this, if possible, rather than checking manually if the task has completed.. But this is just a sample
                    var task = SoapClient.RunTransactionAsync(request);
                    do
                    {
                        Console.WriteLine("Processing task...");
                        Console.WriteLine(Environment.NewLine);
                        Thread.Sleep(150);
                    } while (!task.IsCompleted);

                    reply = task.Result;
                }

                SaveOrderState();
                ProcessReply(reply);
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
            catch (AggregateException ae)
            {
                SaveOrderState();
                foreach (var e in ae.InnerExceptions)
                {
                    HandleException(e);
                }
            }
            //Any other exception!
            catch (Exception e)
            {
                SaveOrderState();
                HandleException(e);
            }

            Console.WriteLine("Press Return to end...");
            Console.ReadLine();
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

        private static void ProcessReply(ReplyMessage reply)
        {
            string template = GetTemplate(reply.decision.ToUpper());
            string content = GetContent(reply);

            /*
			 * Display result of transaction.  Being a console application,
			 * this sample simply prints out some text on the screen.  Use
			 * what is appropriate for your system (e.g. ASP.NET pages).
			 */
            Console.WriteLine(template, content);
        }

        private static string GetTemplate(string decision)
        {
            /*
			 * This is where you retrieve the HTML template that corresponds
			 * to the decision.  This template has 'boiler-plate' wording and
			 * can be stored in files or a database.  This is just one way to
			 * retrieve feedback pages.  Use what is appropriate for your
			 * system (e.g. ASP.NET pages).
			 */

            if ("ACCEPT".Equals(decision))
            {
                return ("The transaction succeeded.{0}");
            }

            if ("REJECT".Equals(decision))
            {
                return ("Your order was not approved.{0}");
            }

            // ERROR
            return (
                "Your order could not be completed at this time.{0}" +
                "\nPlease try again later.");
        }

        private static string GetContent(ReplyMessage reply)
        {
            /*
			 * This is where you retrieve the content that will be plugged
			 * into the template.
			 * 
			 * The strings returned in this sample are mostly to demonstrate
			 * how to retrieve the reply fields.  Your application should
			 * display user-friendly messages.
			 */

            int reasonCode = int.Parse(reply.reasonCode);
            switch (reasonCode)
            {
                // Success
                case 100:
                    return (
                        "\nRequest ID: " + reply.requestID);

                // Missing field(s)
                case 101:
                    return (
                        "\nThe following required field(s) are missing: " +
                        EnumerateValues(reply.missingField));

                // Invalid field(s)
                case 102:
                    return (
                        "\nThe following field(s) are invalid: " +
                        EnumerateValues(reply.invalidField));

                // Insufficient funds
                case 204:
                    return (
                        "\nInsufficient funds in the account.  Please use a " +
                        "different card or select another form of payment.");

                // add additional reason codes here that you need to handle
                // specifically.

                default:
                    // For all other reason codes, return an empty string,
                    // in which case, the template will be displayed with no
                    // specific content.
                    return (String.Empty);
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

        private static void HandleWebException(WebException we)
        {
            string template = GetTemplate("ERROR");

            /*
			 * The string returned in this sample is mostly to demonstrate
			 * how to retrieve the exception properties.  Your application
			 * should display user-friendly messages.
			 */
            string content = String.Format(
                "\nFailed to get a response with status '{0}' and " +
                "message '{1}'", we.Status, we.Message);

            Console.WriteLine(template, content);

            if (IsCriticalError(we))
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

        private static string EnumerateValues(string[] array)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string val in array)
            {
                sb.Append(val + "\n");
            }

            return (sb.ToString());
        }

        private static bool IsCriticalError(WebException we)
        {
            switch (we.Status)
            {
                case WebExceptionStatus.ProtocolError:
                    if (we.Response != null)
                    {
                        HttpWebResponse response
                            = (HttpWebResponse)we.Response;

                        // GatewayTimeout may be returned if you are
                        // connecting through a proxy server.
                        return (response.StatusCode ==
                            HttpStatusCode.GatewayTimeout);

                    }

                    // In case of ProtocolError, the Response property
                    // should always be present.  In the unlikely case 
                    // that it is not, we assume something went wrong
                    // along the way and to be safe, treat it as a
                    // critical error.
                    return (true);

                case WebExceptionStatus.ConnectFailure:
                case WebExceptionStatus.NameResolutionFailure:
                case WebExceptionStatus.ProxyNameResolutionFailure:
                case WebExceptionStatus.SendFailure:
                    return (false);

                default:
                    return (true);
            }
        }
    }
}

public class SampleTransactions
{
    public RequestMessage authRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth

        // Credit Card Authorization
        request.ccAuthService = new CCAuthService();
        request.ccAuthService.run = "true";

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

        return request;
    }

    public RequestMessage authRevRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth

        // Credit Card Authorization

        request.ccAuthReversalService = new CCAuthReversalService();
        request.ccAuthReversalService.run = "true";
        request.ccAuthReversalService.authRequestID = "auth-request-id";

        request.merchantReferenceCode = "your_merchant_reference_code";

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

        return request;

    }

    public RequestMessage emvAuthRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth for emv

        // Credit Card Authorization
        request.ccAuthService = new CCAuthService();
        request.ccAuthService.run = "true";
        request.ccAuthService.commerceIndicator = "retail";

        Pos pos = new Pos();
        pos.entryMode = "contact";
        pos.cardPresent = "Y";
        pos.terminalCapability = "4";
        pos.trackData = "=%B4111111111111111^SMITH/BETTY^16121200123456789012**XXX * *****? *; 4111111111111111 = 16121200XXXX00000000 ? *";
        request.pos = pos;

        EmvRequest emvRequest = new EmvRequest();
        emvRequest.cardSequenceNumber = "001";
        emvRequest.combinedTags = "9F3303204000950500000000009F3704518823719F100706011103A000009F26081E1756ED0E2134E29F36020015820200009C01009F1A0208409A030006219F02060000000020005F2A0208409F0306000000000000";


        request.emvRequest = emvRequest;
        // add required fields

        request.merchantReferenceCode = "your_merchant_reference_code";

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

        return request;

    }

    public RequestMessage level2CaptureRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth

        // Credit Card Authorization

        request.ccCaptureService = new CCCaptureService();
        request.ccCaptureService.run = "true";
        request.ccCaptureService.authRequestID = "capture-request-id";

        request.merchantReferenceCode = "your_merchant_reference_code";

        InvoiceHeader invoiceHeader = new InvoiceHeader();
        invoiceHeader.merchantVATRegistrationNumber = "GB-123445555";
        invoiceHeader.summaryCommodityCode = "SUMM";
        invoiceHeader.supplierOrderReference = "456456346";
        request.invoiceHeader = invoiceHeader;

        PurchaseTotals purchaseTotals = new PurchaseTotals();
        purchaseTotals.currency = "USD";
        request.purchaseTotals = purchaseTotals;

        // there are two items in this sample
        request.item = new Item[1];

        Item item = new Item();
        item.id = "0";
        item.unitPrice = "1.99";
        item.taxAmount = "1.00";
        item.taxRate = "0.05";
        item.nationalTax = "0.01";
        item.totalAmount = "9.95";
        item.quantity = "5";
        item.commodityCode = "2222";
        item.alternateTaxRate = "0.01";
        request.item[0] = item;

        return request;

    }

    public RequestMessage paymentNetworkTokenizationRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests

        // Credit Card Authorization
        request.ccAuthService = new CCAuthService();
        request.ccAuthService.run = "true";
        request.ccAuthService.commerceIndicator = "vbv";
        request.ccAuthService.cavv = "EHuWW9PiBkWvqE5juRwDzAUFBAk=";
        request.ccAuthService.xid = "EHuWW9PiBkWvqE5juRwDzAUFBAk=";

        request.merchantReferenceCode = "your_merchant_reference_code";

        UCAF ucaf = new UCAF();
        ucaf.authenticationData = "EHuWW9PiBkWvqE5juRwDzAUFBAk=";
        ucaf.collectionIndicator = "2";
        request.ucaf = ucaf;

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
        card.accountNumber = "4650100000000839";
        card.expirationMonth = "12";
        card.expirationYear = "2031";
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

        PaymentNetworkToken paymentNetworkToken = new PaymentNetworkToken();
        paymentNetworkToken.transactionType = "1";

        request.paymentNetworkToken = paymentNetworkToken;

        return request;

    }
    public RequestMessage refundRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests for refund

        // Credit Card Authorization

        request.ccCreditService = new CCCreditService();
        request.ccCreditService.run = "true";
        request.ccCreditService.reconciliationID = "7189912a5HVUHCYNR";
        request.merchantReferenceCode = "your_merchant_reference_code";
        request.ccCreditService.refundReason = "1";

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

        return request;

    }

    public RequestMessage visaCheckoutAuthRequest()
    {
        RequestMessage request = new RequestMessage();
        request.getVisaCheckoutDataService = new GETVisaCheckoutDataService();
        request.getVisaCheckoutDataService.run = "true";
        request.merchantReferenceCode = "visacheckoutemssail09";
        request.paymentSolution = "visacheckout";
        VC vc = new VC();
        vc.orderID = "1541325474577083201";
        request.vc = vc;

        return request;

    }

    public RequestMessage androidPayAuthRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth for android pay

        // Credit Card Authorization
        request.ccAuthService = new CCAuthService();
        request.ccAuthService.run = "true";
        request.ccAuthService.cavv = "ABCDEFabcdefABCDEFabcdef0987654321234567";
        request.ccAuthService.commerceIndicator = "internet";
        request.ccAuthService.xid = "ABCDEFabcdefABCDEFabcdef0987654321234567";

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

        PaymentNetworkToken paymentNetworkToken = new PaymentNetworkToken();
        paymentNetworkToken.transactionType = "1";
        request.paymentNetworkToken = paymentNetworkToken;

        request.paymentSolution = "006";
        // add optional fields here per your business needs

        return request;


    }

    public RequestMessage applePayAuthRequest()
    {
        RequestMessage request = new RequestMessage();

        // we will let the client pick up the merchantID
        // from the config file.  In multi-merchant scenarios,
        // you would set a merchantID in each request.

        // this sample requests auth of apple pay

        // Credit Card Authorization
        request.ccAuthService = new CCAuthService();
        request.ccAuthService.run = "true";
        request.ccAuthService.cavv = "ABCDEFabcdefABCDEFabcdef0987654321234567";
        request.ccAuthService.commerceIndicator = "internet";
        request.ccAuthService.xid = "ABCDEFabcdefABCDEFabcdef0987654321234567";

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

        PaymentNetworkToken paymentNetworkToken = new PaymentNetworkToken();
        paymentNetworkToken.transactionType = "1";
        request.paymentNetworkToken = paymentNetworkToken;
        request.paymentSolution = "001";
        // add optional fields here per your business needs

        return request;
    }

    public RequestMessage saleRequest()
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

        return request;
    }
}
