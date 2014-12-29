using System;
using System.Collections;
using System.IO;
using System.Configuration;
using System.Threading;
using System.Text;
//using System.Web.Services.Protocols;
using System.Xml;

namespace CyberSource.Base
{
    public class Logger
	{
        public enum LogType
        {
            FILESTART, TRANSTART, CONFIG, REQUEST, REPLY, FAULT, EXCEPTION
        };

        private static Mutex smMutex = new Mutex(false, "CyberSource.Base.Logger");
        private const int MUTEX_TIMEOUT = 10000; // in milliseconds

        private static bool forceArchive;
        private static Type WebExceptionType = typeof(System.Net.WebException);

        static Logger()
        {
            // if you want test the archiving feature without needing to
            // reach the maximum size, include forceArchive in the config
            // file and set it to anything.
            //
            // Alternatively, you can just set maxLogSizeInMB (which I just
            // recently added) to zero.
            forceArchive 
             = (ConfigurationManager.AppSettings["forceArchive"] != null);
        }

        private const long MB = 1048576;
        private const string TRANSTART_MARKER = "===============================================================================";
        private const string NULL_ENTRY = "(null)";
        private const string FILESTART_ENTRY = "CYBERSOURCE LOG FILE";
        private const string TRANSTART_ENTRY = "";

        private const string LINEFEED = "\n";
        private const string CARRIAGE_RETURN = "\r";

        // For some reason, the compiler didn't want the
        // Environment.NewLine as my initializer here.
        private const string NEWLINE = "\r\n"; // + Environment.NewLine;

        // 0:Year,1:Month,2:Date,3:Hour,4:Minute,5:Second
        private const string ARCHIVE_EXT_FORMAT = ".{0:D4}-{1:D2}-{2:D2}T{3:D2}{4:D2}{5:D2}";

        // 0:Year,1:Month,2:Date,3:Hour,4:Minute,5:Second,6:Millisecond,7:Thread HashCode,8:Transaction Type,9:Optional Newline,10:text,11:Newline
        private const string LOG_ENTRY_FORMAT  = "{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3} {7:D5} {8}> {9}{10}{11}";

        private const string DEMO_REQUEST_HEADER = "REQUEST:" + NEWLINE;
        private const string DEMO_REPLY_HEADER   = "REPLY:" + NEWLINE;

        private string mPath;

        private Logger() { /* use GetInstance() instead */ }

        public static Logger GetInstance(
            string directory, string filename, int maxSizeInMB)
        {
            if (AcquireMutexWithTimeout())
            {
                FileStream stream = null;
                try
                {
                    Logger logger = new Logger();
                    logger.mPath = Path.Combine(directory, filename);

                    stream
                        = new FileStream(
                        logger.mPath, FileMode.Append, FileAccess.Write,
                        FileShare.ReadWrite);

                    long size = stream.Length;

                    // include forceArchive in the config file and set it
                    // to anything to test this block of code without
                    // needing to reach the actual maximum size.
                    if ((size > maxSizeInMB * MB) || forceArchive)
                    {
                        stream.Close();
                        File.Move(logger.mPath, FormatArchiveName(logger.mPath));

                        stream
                            = new FileStream(
                            logger.mPath, FileMode.CreateNew, FileAccess.Write,
                            FileShare.ReadWrite);

                        size = 0;
                    }

                    if (size == 0)
                    {
                        logger.Log(LogType.FILESTART, FILESTART_ENTRY);
                    }

                    return logger;
                }
                catch (System.IO.IOException)
                {
                    return null;
                }
                finally
                {
                    CloseStreamAndReleaseMutex(stream);
                }
            }

            return null;
        }

        public void Log( LogType type, string text )
        {
            if (AcquireMutexWithTimeout())
            {
                FileStream stream = null;
                try
                {
                    stream 
                        = new FileStream(
                               mPath, FileMode.Append, FileAccess.Write,
                               FileShare.ReadWrite );

                    if (type == LogType.TRANSTART) {
                        LogString( stream, NEWLINE + TRANSTART_MARKER + NEWLINE );
                    }

                    string logText = text != null ? text : NULL_ENTRY;
                    if (logText.Contains(LINEFEED) &&
                        !logText.Contains(CARRIAGE_RETURN))
                    {
                        logText = logText.Replace(LINEFEED, NEWLINE);
                    }
                    LogString(stream, FormatEntry(type, logText));
                }
                catch (Exception)
                {
                    // ignore any exceptions.  We don't want logging
                    // failures (except for the preparation of the
                    // log file) to fail on-going transactions.
                }
                finally
                {
                    CloseStreamAndReleaseMutex(stream);
                }
            }
        }

        public void LogTransactionStart( string configString )
        {
            if (AcquireMutexWithTimeout())
            {
                try
                {

                    Log(LogType.TRANSTART, TRANSTART_ENTRY);
                    Log(LogType.CONFIG, configString);
                }
                finally
                {
                    smMutex.ReleaseMutex();
                }
            }
        }

        public void LogException(Exception e)
        {
            string webExceptionStatus = String.Empty;
            if (WebExceptionType.Equals(e.GetType()))
            {
                System.Net.WebException we = (System.Net.WebException)e;
                webExceptionStatus = " " + we.Status;
            }

            Log(
                LogType.EXCEPTION,
                e.GetType().FullName + ":" +
                webExceptionStatus + NEWLINE +
                e.Message + NEWLINE +
                e.StackTrace);
        }
        

        public void LogException(string logString)
        {
            Log(LogType.EXCEPTION, logString);
        }

        public void LogFault(string logString)
        {
            Log(LogType.FAULT, logString);
        }

        //public void LogSoapException(SoapException se)
        //{
        //    Log(
        //        LogType.FAULT,
        //        se.Code + NEWLINE +
        //        se.Message + NEWLINE +
        //        "Detail: " +
        //        (se.Detail != null ? se.Detail.OuterXml : "<null>"));
        //}

        public void LogRequest(Hashtable request, bool demo)
        {
            string logString
                = GetLogString(SafeFields.MessageType.REQUEST, request);

            if (demo)
            {
                Console.WriteLine( DEMO_REQUEST_HEADER + logString + NEWLINE );
            }
            Log(LogType.REQUEST, logString );
        }

        public void LogReply(Hashtable reply, bool demo)
        {
            string logString
                = GetLogString(SafeFields.MessageType.REPLY, reply);

            if (demo)
            {
                Console.WriteLine(DEMO_REPLY_HEADER + logString + NEWLINE);
            }

            Log(LogType.REPLY, logString );
        }

        public void LogRequest(XmlNode request, bool demo)
        {
            request = request.CloneNode(true);
            MaskXml(SafeFields.MessageType.REQUEST, request, null);
            string logString = request.OuterXml;

            if (demo)
            {
                Console.WriteLine(DEMO_REQUEST_HEADER + logString + NEWLINE);
            }

            Log(LogType.REQUEST, logString );
        }

        public void LogReply(XmlNode reply, bool demo)
        {
            reply = reply.CloneNode(true);
            MaskXml(SafeFields.MessageType.REPLY, reply, null);
            string logString = reply.OuterXml;

            if (demo)
            {
                Console.WriteLine(DEMO_REPLY_HEADER + logString + NEWLINE);
            }

            Log(LogType.REPLY, logString );
        }

        private const string UNDERSCORE = "_";

        // CyberSource "root" fields
        private const string CYBS_ROOT_FIELDS = "requestMessage replyMessage nvpRequest nvpReply";

        // parentName must be null.  This is a recursive method.
        // The recursive calls will pass non-NULL strings to said
        // parameter.
        private static void MaskXml(
            SafeFields.MessageType type, XmlNode node,
            string parentName)
        {
            if (node == null) return;

            XmlNodeType nodeType = node.NodeType;
            if (nodeType == XmlNodeType.Text)
            {
                if (!SafeFields.IsSafe(type, parentName))
                {
                    string origVal = node.Value;
                    if (origVal != null) origVal = origVal.Trim();
                    if (origVal != null && origVal.Length > 0)
                    {
                        node.Value = MaskedValue(parentName, origVal);
                    }
                }
            }
            else if (nodeType == XmlNodeType.Element ||
                     nodeType == XmlNodeType.Document)
            {
                if (!node.HasChildNodes) return;

                String localName = node.LocalName;
                if (localName == null) localName = String.Empty;

                String fieldFullName = null;
                if (parentName == null)
                {
                    // we have not encountered any of the fields in
                    // CYBS_ROOT_FIELDS, in which case, we check if
                    // the current node's local name is one of them.
                    // If so, then we set fieldFullName to "".
                    // Otherwise, fieldFullName remains null.
                    if (localName.Length > 0 &&
                        CYBS_ROOT_FIELDS.IndexOf(localName) != -1)
                    {
                        fieldFullName = "";
                    }
                }
                else if (parentName.Length == 0)
                {
                    // the immediate parent of this node is one of
                    // those in CYBS_ROOT_FIELDS, in which case, we
                    // use its local name as the field name so far.
                    fieldFullName = localName;
                }
                else
                {
                    // this is a node that is at least two levels
                    // down from one of the CYBS_ROOT_FIELDS, in which
                    // case, we append its local name to the parent's name.
                    fieldFullName = parentName + "_" + localName;
                }

                // call this method recursively on each of the child nodes
                XmlNodeList children = node.ChildNodes;
                foreach (XmlNode child in children)
                {
                    MaskXml(type, child, fieldFullName);
                }
            }
        }

        private static bool AcquireMutexWithTimeout()
        {
            return (smMutex.WaitOne(MUTEX_TIMEOUT, false));
        }

        private static void CloseStreamAndReleaseMutex(FileStream stream)
        {
            try
            {
                if (stream != null)
                    stream.Close();
            }
            catch (IOException)
            {
                // ignore
            }
            finally
            {
                smMutex.ReleaseMutex();
            }
        }

        private void LogString(FileStream stream, string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static string FormatArchiveName( string path )
        {
            // get current date and time
            DateTime now = DateTime.Now;
            string ext = String.Format(
                    ARCHIVE_EXT_FORMAT,
                    now.Year, now.Month, now.Day,
                    now.Hour, now.Minute, now.Second );
            return( path + ext );
        }

        private string FormatEntry( LogType type, string text )
        {
            // get current date and time
            DateTime now = DateTime.Now;
            string entry = String.Format(
                    LOG_ENTRY_FORMAT,
                    now.Year, now.Month, now.Day, now.Hour,
                    now.Minute, now.Second, now.Millisecond,
                    System.Threading.Thread.CurrentThread.GetHashCode(),
                    type.ToString().PadRight( 10 ),
                    text.Contains( LINEFEED ) ? NEWLINE : String.Empty,
                    text, NEWLINE);
            return( entry );
        }

        private const char NV_SEPARATOR = '=';
        private const string NVP_SEPARATOR = NEWLINE;
        private static string GetLogString(
            SafeFields.MessageType type, Hashtable table)
        {
            if (table == null || table.Count == 0) return( String.Empty );

            StringBuilder sb = new StringBuilder();

            foreach (string key in table.Keys) {
                if (sb.Length > 0) sb.Append(NVP_SEPARATOR);
                sb.Append(key);
                sb.Append(NV_SEPARATOR);
                sb.Append(
                    SafeFields.IsSafe( type, key )
                        ? (string) table[key]
                        : MaskedValue( key, (string) table[key] ) );
            }

            return (sb.ToString());
        }

        private const string TRACK_DATA = "trackData";
        private const char MASK_CHAR = 'x';
        public static string MaskedValue(string key, string val)
        {
            if (val == null) return (NULL_ENTRY);

            int len = val.Length;
            if (len == 0) return (String.Empty);

            if (key.Contains(TRACK_DATA) ||
                len >= 1 && len <= 9)
            {
                // mask everything
                return( new String( MASK_CHAR, val.Length ) );
            }

            if (len >= 10 && len <= 15)
            {
                // mask everything but the first and last two
                return( 
                    val.Substring(0, 2) +
                    (new String(MASK_CHAR, len - 4)) +
                    val.Substring(len - 2));              
            }

            // for strings that have 16 chars or more,
            // mask everything but the first and last four
            return (
                val.Substring(0, 4) +
                (new String(MASK_CHAR, len - 8)) +
                val.Substring(len - 4));
        }
    }   // Logger class
} // namespace