
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.IO;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.XPath;

namespace CyberSource.Clients
{
    public class CustomTextMessageEncoder : MessageEncoder
    {
        private CustomTextMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;
        private byte[] key;
   
        public CustomTextMessageEncoder(CustomTextMessageEncoderFactory factory)
        {
            this.factory = factory;
            
            this.writerSettings = new XmlWriterSettings();            
        }

        public override string ContentType
        {
            get
            {
                return "text/xml";
            }
        }

        public override string MediaType
        {
            get 
            {
                return "text/xml";
            }
        }

        public override MessageVersion MessageVersion
        {
            get 
            {
                return this.factory.MessageVersion;
            }
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {   
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            String wireResponse;

            using (var sr = new StreamReader(stream))
            {
                wireResponse = sr.ReadToEnd();
            }
             
            // Fix for Xml external entity injection violation in fortify report
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Prohibit;
            settings.XmlResolver = null;

            XmlDocument doc = new XmlDocument();
            Message returnMessage = null;

            using (StringReader stringReader = new StringReader(wireResponse))
            {
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    doc.Load(reader);

                    //We need to get rid of the security header because it is not signed by the web service.
                    //The whole reason for the custom Encoder is to do this. the client rejected the unsigned header.
                    //Our WCF client is set up to allow the absence of a security header but if the header exists then it must be signed.
                    //Hopefully the namespace will not change. Maybe it should be put in a config.

                    XPathNavigator n = doc.CreateNavigator();
                    if (n.MoveToFollowing("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"))
                    {
                        n.DeleteSelf();
                    }

                    StringReader stringReaderInnerXml = new StringReader(doc.InnerXml);
                    XmlReader reader2 = XmlReader.Create(stringReaderInnerXml, settings);
                    returnMessage = Message.CreateMessage(reader2, maxSizeOfHeaders, MessageVersion.Soap11);
                }
            }

            return returnMessage;
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            return byteArray;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }
}
