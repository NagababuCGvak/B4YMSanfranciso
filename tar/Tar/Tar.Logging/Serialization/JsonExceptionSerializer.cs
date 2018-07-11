using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Tar.Logging.Serialization
{
    internal class JsonExceptionSerializer : IExceptionSerializer
    {
        public string Serialize<T>(T exception) where T : Exception
        {
            var sb = new StringBuilder();
            
            var root = XDocument.Parse(ExceptionSerializerFactory.CreateSerializer(ExceptionSerializerType.Xml)
                                           .Serialize(exception));
            var xElement = root.Element("Exception");

            sb.Append("Exception{");
            SerializeException(xElement, sb);
            sb.Append("}");
            return sb.ToString();
        }

        private void SerializeException(XElement xElement, StringBuilder sb)
        {
            if (xElement != null)
            {
                var first = true;
                xElement.Elements().Where(el => el.Name != "Data" && el.Name != "InnerException").ToList().ForEach(
                    el =>
                        {
                            sb.AppendFormat(first ? "'{0}':'{1}'" : "',{0}':'{1}'", el.Name, el.Value);
                            first = false;
                        });

                var dataElement = xElement.Elements().FirstOrDefault(el => el.Name == "Data");
                if (dataElement != null)
                {
                    sb.Append(",'Data':{");
                    sb.AppendFormat("Count:{0}", dataElement.Elements().Count());

                    if (dataElement.HasElements)
                        dataElement.Elements().ToList().ForEach(
                            el => sb.AppendFormat(",'{0}':'{1}'", el.Name, el.Value));

                    sb.Append("}");
                }
                else
                {
                    sb.AppendFormat(",'{0}':{1}", "Data", "{Count=0}");
                }

                var innerExceptionElement = xElement.Elements().First(el => el.Name == "InnerException");

                sb.AppendFormat(",'{0}':", "InnerException"); 

                if (innerExceptionElement.HasElements)
                    SerializeException(innerExceptionElement, sb);
                else
                    sb.Append("null");
            }
            else
                sb.AppendLine("Exception elementi bulunamad�!");
            sb.Append("}");
        }
    }
}