using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tar.Core.Serialization.SoapManagement
{
    public static class SerializationHelper
    {
        public static string ToXml(object data)
        {
            var json = ToJson(data);
            var xml = ToXmlFromJson(json);
            return xml;
        }

        public static string ToXmlFromJson(string json)
        {
            var doc = ToXmlDocument(json);
            return doc.InnerXml;
        }

        public static XmlDocument ToXmlDocument(string json)
        {
            return JsonConvert.DeserializeXmlNode(json);
        }

        public static XDocument ToXDocument(string json)
        {
            return JsonConvert.DeserializeXNode(json);
        }

        public static string ToJson(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return json;
        }
        public static string ToJsonFromXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var json = JsonConvert.SerializeObject(xmlDoc);
            return json;
        }

        public static JObject ToJObjectFromJson(string json)
        {
            return (JObject)JsonConvert.DeserializeObject(json);
        }

        public static JObject ToJObjectFromXml(string xml)
        {
            return (JObject)JsonConvert.DeserializeObject(ToJsonFromXml(xml));
        }

        public static T ToObjectFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Transform(string xsl, object parameters)
        {
            return Transform(xsl, ToXml(parameters));
        }

        public static string Transform(string xsl, string inputXml)
        {
            var doc = new XPathDocument(new StringReader(inputXml));
            var output = new StringBuilder();
            var xslReader = XmlReader.Create(new StringReader(xsl));

            var transform = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };
            transform.Load(xslReader, settings, null);
            transform.Transform(doc, new XsltArgumentList(), new StringWriter(output));
            var xDocument = XDocument.Load(new StringReader(output.ToString()));
            return xDocument.ToString();
        }

        public static string RemoveNamespaces(string xml)
        {
            var doc = new XPathDocument(new StringReader(xml));
            var output = new StringBuilder();
            var xslReader = XmlReader.Create(new StringReader(RemoveNamespacesXslt));
            var transform = new XslCompiledTransform();
            var settings = new XsltSettings { EnableScript = true };
            transform.Load(xslReader, settings, null);
            transform.Transform(doc, new XsltArgumentList(), new StringWriter(output));
            var xDocument = XDocument.Load(new StringReader(output.ToString()));
            return xDocument.ToString();
        }

        private const string RemoveNamespacesXslt = @"<?xml version='1.0'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>

    <xsl:output indent='yes' method='xml' encoding='utf-8' omit-xml-declaration='yes'/>

    <!-- Stylesheet to remove all namespaces from a document -->
    <!-- NOTE: this will lead to attribute name clash, if an element contains
        two attributes with same local name but different namespace prefix -->
    <!-- Nodes that cannot have a namespace are copied as such -->

    <!-- template to copy elements -->
    <xsl:template match='*'>
        <xsl:element name='{local-name()}'>
            <xsl:apply-templates select='@* | node()'/>
        </xsl:element>
    </xsl:template>

    <!-- template to copy attributes -->
    <xsl:template match='@*'>
        <xsl:attribute name='{local-name()}'>
            <xsl:value-of select='.'/>
        </xsl:attribute>
    </xsl:template>

    <!-- template to copy the rest of the nodes -->
    <xsl:template match='comment() | text() | processing-instruction()'>
        <xsl:copy/>
    </xsl:template>

</xsl:stylesheet>";
    }
}