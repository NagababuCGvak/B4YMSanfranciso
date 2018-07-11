using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tar.Core.LoggingOld2.Serialization
{
    /// <summary/>
    internal class XmlExceptionSerializer : IExceptionSerializer
    {
        public string Serialize<T>(T exception) where T : Exception
        {
            var root = new XElement("Exception");

            SerializeException(root, exception);

            return root.ToString();
        }

        private void SerializeException(XContainer root, Exception exception)
        {
            if (root == null) throw new ArgumentNullException("root");
            SerializeExceptionProperties(root, exception);

            var innerExceptionElement = new XElement("InnerException");
            root.Add(innerExceptionElement); 
            
            if (exception.InnerException == null) return;
            SerializeException(innerExceptionElement, exception.InnerException);
        }

        private void SerializeExceptionProperties(XContainer root, Exception exception)
        {
            if (root == null) throw new ArgumentNullException("root");
            var exceptionType = exception.GetType();
            root.Add(new XElement("Type", exceptionType.ToString()));
            root.Add(new XElement("Assemly", exception.GetType().Assembly.GetName().Name));

            var propertyInfos = exception.GetType().GetProperties()
                .Where(propertyInfo => propertyInfo.CanRead &&
                                       IsAllowType(propertyInfo.PropertyType)).ToList();

            propertyInfos.ForEach(
                propertyInfo => root.Add(new XElement(propertyInfo.Name, propertyInfo.GetValue(exception, null))));

            exception.GetType().GetProperties()
                .Where(propertyInfo => propertyInfo.CanRead &&
                                       propertyInfo.PropertyType.IsAssignableFrom(typeof (IDictionary))).ToList()
                .ForEach(propertyInfo =>
                             {
                                 var data = new XElement(propertyInfo.Name);
                                 SerializeDictionary(data, (IDictionary)propertyInfo.GetValue(exception, null));
                                 root.Add(data);
                             });
        }

        private readonly List<Type> _allowedTypes = new List<Type>();

        private bool IsAllowType(Type type)
        {
            return _allowedTypes.Contains(type);
        }

        protected void SerializeDictionary(XElement root, IDictionary data)
        {
            if (root == null)
                throw new ArgumentNullException("root"); 
            
            var count = (data == null ? 0 : data.Count);
            var countAttribute = new XAttribute("Count", count);
            root.Add(countAttribute);

            if (data == null || data.Count == 0) return;
            
            new DictionaryXmlSerializer(data).Serialize(root);
        }

        internal XmlExceptionSerializer()
        {
            _allowedTypes.Add(typeof(string));
            _allowedTypes.Add(typeof(char));
            _allowedTypes.Add(typeof(Int16));
            _allowedTypes.Add(typeof(Int32));
            _allowedTypes.Add(typeof(Int64));
            _allowedTypes.Add(typeof(Double));
            _allowedTypes.Add(typeof(Decimal));
        }
    }
}