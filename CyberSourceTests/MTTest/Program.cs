using System;
using System.Collections;
using System.Net;
using System.Threading;
using CyberSource.Clients;

namespace CyberSource.Test
{
    class MTTest
    {
        private static int threadCount = 0;

        static void Main(string[] args)
        {
            //ServicePointManager.DefaultConnectionLimit = 5;

            ThreadStart entry = new ThreadStart(ThreadMethod);

            int numThreads
                = (args != null && args.Length > 0)
                    ? Int32.Parse( args[0] ) : 1;

            Console.WriteLine("Number of Threads: " + numThreads);

            for (int i = 0; i < numThreads; ++i)
            {
                Thread thread1 = new Thread(entry);
                thread1.Start();
            }

            while (threadCount > 0)
            {
                Thread.Sleep(5000);
            }

            Console.WriteLine("Second Iteration");
            for (int i = 0; i < numThreads; ++i)
            {
                Thread thread1 = new Thread(entry);
                thread1.Start();
            }
        }

        public static void ThreadMethod()
        {
            Interlocked.Increment(ref threadCount);

            Hashtable request = new Hashtable();

            // we will let the client pick up the merchantID
            // from the config file.  In multi-merchant scenarios,
            // you would set a merchantID in each request.

            // this sample requests both auth and capture

            // Credit Card Authorization
            request.Add("ccAuthService_run", "true");

            // Credit Card Capture
            request.Add("ccCaptureService_run", "true");

            // add required fields

            request.Add("merchantReferenceCode", "your_merchant_reference_code");
            request.Add("billTo_firstName", "John");
            request.Add("billTo_lastName", "Doe");
            request.Add("billTo_street1", "1295 Charleston Road");
            request.Add("billTo_city", "Mountain View");
            request.Add("billTo_postalCode", "94043");
            request.Add("billTo_state", "CA");
            request.Add("billTo_country", "US");
            request.Add("billTo_email", "nobody@cybersource.com");
            request.Add("billTo_ipAddress", "10.7.111.111");
            request.Add("card_accountNumber", "4111111111111111");
            request.Add("card_expirationMonth", "12");
            request.Add("card_expirationYear", "2030");
            request.Add("purchaseTotals_currency", "USD");

            // there are two items in this sample
            request.Add("item_0_unitPrice", "12.34");
            request.Add("item_1_unitPrice", "56.78");

            // add optional fields here per your business needs

            try
            {
                WriteStatus("Before RunTransaction");
                Hashtable reply = NVPClient.RunTransaction(request);
                WriteStatus("After RunTransaction");
                SaveOrderState();
                ProcessReply(reply);
            }
            catch (WebException we)
            {
                SaveOrderState();
                HandleWebException(we);
            }
            catch (Exception e)
            {
                SaveOrderState();

            }
            finally
            {
                Interlocked.Decrement(ref threadCount);
            }
        }

        private static void WriteStatus(string text)
        {
            DateTime now = DateTime.Now;

            Console.WriteLine(  
                "" + now.Hour + ":" + now.Minute + ":" + now.Second + "." +
                now.Millisecond + "> " +
                Thread.CurrentThread.GetHashCode() + ": " + text);
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

        private static void ProcessReply(Hashtable reply)
        {
            string template = GetTemplate(
                                ((string)reply["decision"]).ToUpper());
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

        private static string GetContent(Hashtable reply)
        {
            /*
             * This is where you retrieve the content that will be plugged
             * into the template.
             * 
             * The strings returned in this sample are mostly to demonstrate
             * how to retrieve the reply fields.  Your application should
             * display user-friendly messages.
             */

            int reasonCode = int.Parse((string)reply["reasonCode"]);
            switch (reasonCode)
            {
                // Success
                case 100:
                    return (
                        "\nRequest ID: " + reply["requestID"] +
                        "\nAuthorization Code: " +
                            reply["ccAuthReply_authorizationCode"] +
                        "\nCapture Request Time: " +
                            reply["ccCaptureReply_requestDateTime"] +
                        "\nCaptured Amount: " +
                            reply["ccCaptureReply_amount"]);

                // Missing field(s)
                case 101:
                    return (
                        "\nThe following required field(s) are missing: " +
                        EnumerateValues(reply, "missingField"));

                // Invalid field(s)
                case 102:
                    return (
                        "\nThe following field(s) are invalid: " +
                        EnumerateValues(reply, "invalidField"));

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

        private static string EnumerateValues(
            Hashtable reply, string fieldName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string val = "";
            for (int i = 0; val != null; ++i)
            {
                val = (string)reply[fieldName + "_" + i];
                if (val != null)
                {
                    sb.Append(val + "\n");
                }
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
