using System;
using System.Collections;
using System.Xml.Linq;

namespace Tar.Core.LoggingOld2.Serialization
{
    /// <summary/>
    public class DictionaryXmlSerializer
    {
        private readonly IDictionary _data;

        /// <summary/>
        public DictionaryXmlSerializer(IDictionary data)
        {
            if (data == null) throw new ArgumentNullException("data");
            _data = data;
        }

        /// <summary/>
        public void Serialize(XElement root)
        {
            foreach (DictionaryEntry item in _data)
                root.Add(new XElement(item.Key.ToString(), item.Value));
        }
        
        /// <summary/>
        public string Serialize(string elementName)
        {
            var element = new XElement(elementName);
            Serialize(element);
            return element.ToString();
        }
    }
}