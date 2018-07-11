using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tar.Core.Serialization.SoapManagement
{
    public static class SoapHelper
    {
        private static IDictionary<int, ISoapMessageConverter> Converters { get; set; }

        static SoapHelper()
        {
            Converters = new Dictionary<int, ISoapMessageConverter>();
            AddConverter(new DefaultSoapMessageConverter());
            AddConverter(new JObjectSoapMessageConverter());
            AddConverter(new XmlDocumentSoapMessageConverter());
            AddConverter(new PureXmlSoapMessageConverter());
            AddConverter(new PureJsonSoapMessageConverter());

        }
        public static void AddConverter(ISoapMessageConverter converter)
        {
            if (!Converters.Any()) Converters.Add(1, converter);
            else
            {
                var max = Converters.Max(x => x.Key) ;
                var id = max + 1;
                Converters.Add(id, converter);
            }
        }

        public static ISoapMessageConverter GetConverter<T>() where T : class, new()
        {
            var converter = Converters.OrderByDescending(x => x.Key)
                .FirstOrDefault(x => x.Value.CanConvert<T>());
            return converter.Value;
        }

        public static T Convert<T>(string xml) where T : class, new()
        {
            var converter = GetConverter<T>();
            return converter.Convert<T>(xml);
        }

        public static TResponse Execute<TResponse>(SoapParameter parameter, object input)
            where TResponse : class, new()
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            if (input == null) throw new ArgumentNullException("input");
            if (input.GetType().GetProperty("request") == null)
                input = new {request = input};

            var soapMessage = SerializationHelper.Transform(parameter.InputMessageXslt, input);
            var buffer = Encoding.UTF8.GetBytes(soapMessage);
            var request = (HttpWebRequest)WebRequest.Create(parameter.EndPoint);
            request.UseDefaultCredentials = true;

            request.Headers.Add(@"SOAPAction", parameter.SoapAction);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            request.ContentLength = buffer.Length;

            var post = request.GetRequestStream();
            post.Write(buffer, 0, buffer.Length);
            post.Close();
            var response = (HttpWebResponse)request.GetResponse();

            var responseStream = response.GetResponseStream();
            if (responseStream == null) throw new NullReferenceException("responseStream");

            using (var reader = new StreamReader(responseStream))
            {
                var xml = reader.ReadToEnd();
                xml = SerializationHelper.RemoveNamespaces(xml);
                xml = SerializationHelper.Transform(parameter.OutputMessageXslt, xml);
                return Convert<TResponse>(xml);
            }
        }
        public static TResponse Execute<TResponse>(SoapParameter parameter, string inputXml)
           where TResponse : class, new()
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            if (inputXml == null) throw new ArgumentNullException("inputXml");

            var soapMessage = SerializationHelper.Transform(parameter.InputMessageXslt, inputXml);
            var buffer = Encoding.UTF8.GetBytes(soapMessage);
            var request = (HttpWebRequest)WebRequest.Create(parameter.EndPoint);
            request.UseDefaultCredentials = true;

            request.Headers.Add(@"SOAPAction", parameter.SoapAction);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";

            request.ContentLength = buffer.Length;

            var post = request.GetRequestStream();
            post.Write(buffer, 0, buffer.Length);
            post.Close();
            var response = (HttpWebResponse)request.GetResponse();

            var responseStream = response.GetResponseStream();
            if (responseStream == null) throw new NullReferenceException("responseStream");

            using (var reader = new StreamReader(responseStream))
            {
                var xml = reader.ReadToEnd();
                xml = SerializationHelper.RemoveNamespaces(xml);
                xml = SerializationHelper.Transform(parameter.OutputMessageXslt, xml);
                return Convert<TResponse>(xml);
            }
        }
    }
}