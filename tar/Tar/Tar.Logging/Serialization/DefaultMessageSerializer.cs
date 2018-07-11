using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Reflection;

namespace Tar.Logging.Serialization
{
    class DefaultMessageSerializer : IMessageSerializer
    {
        private readonly JavaScriptSerializer _serializer;

        public DefaultMessageSerializer()
        {
            _serializer = new JavaScriptSerializer();
            _serializer.RegisterConverters(new JavaScriptConverter[] { new JsDateTimeConverter() });
        }

        #region Implementation of ISerializer

        public event EventHandler<MessageSerializerEventArgs> Serialized;

        public void OnSerialized(MessageSerializerEventArgs e)
        {
            var handler = Serialized;
            if (handler != null) handler(this, e);
        }

        public virtual string Serialize(object message)
        {
            string serializedMessage;
           
            if (message != null)
            {
                if (message is string)
                {
                    serializedMessage = message.ToString();
                }
                else if (message is Exception)
                {
                    serializedMessage = ((Exception)message).ToXmlString();
                }
                else if (message is DataSet)
                {
                    serializedMessage = ((DataSet) message).GetXml();
                }
                //else if (message.GetType().Name.Contains("AnonymousType")
                //    || message.GetType().Name.EndsWith("Request")
                //    || message.GetType().Name.EndsWith("Response"))
                //{
                //    try
                //    {
                //        var dic = ToDictionary(message);
                //        if (dic.ContainsKey("Exception")
                //            && dic["Exception"] is Exception)
                //        {
                //            dic["Exception"] = ((Exception)dic["Exception"]).ToXmlString();
                //        }
                //        var element = CreateXElement("Message", dic);
                //        serializedMessage = element.ToString();
                //    }
                //    catch
                //    {
                //        serializedMessage = "Message was not serialize for type#" + message.GetType();
                //    }
                //}
                else
                {
                    try
                    {
                        var dic = ToDictionary(message);
                        if (dic.ContainsKey("Exception")
                            && dic["Exception"] is Exception)
                        {
                            dic["Exception"] = ((Exception)dic["Exception"]).ToXmlString();
                        }
                        var element = CreateXElement("Message", dic);
                        serializedMessage = element.ToString();
                    }
                    catch
                    {
                        serializedMessage = "Message was not serialize for type#" + message.GetType();
                    }
                }
            }
            else
                serializedMessage = "null";

            if(string.IsNullOrEmpty(serializedMessage))
                serializedMessage = "null";

            serializedMessage = serializedMessage.StartsWith("<Message>")
                         ? serializedMessage
                         : string.Format("<Message>{0}</Message>", serializedMessage);

            var eventParameter = new MessageSerializerEventArgs(message, serializedMessage);
            OnSerialized(eventParameter);
            return eventParameter.SerializedMessage;
        }

        private XElement CreateXElement(string name, Dictionary<string, object> dic)
        {
            return new XElement(name, dic.Select(item => CreateXNode(item.Key, item.Value)).ToList());
        }

        private XNode CreateXNode(string name, object obj)
        {
            if (obj == null) return new XElement(name, new XText("null"));
            try
            {
                // Hata olu�mas� durumunda sistemin durmamas� i�in try-catch blo�u eklendi.
                if (obj is Dictionary<string, object>)
                    return CreateXElement(name, (Dictionary<string, object>) obj);
                if (obj is Exception)
                    return new XElement(name, new XText(((Exception) obj).ToXmlString()));
                if (obj is DataSet)
                {
                    var element = XElement.Parse(((DataSet) obj).GetXml());
                    element.Name = name;
                    return element;
                }
                
                var objAsString = obj as string;
                if (objAsString != null && objAsString.StartsWith("<") && objAsString.EndsWith(">"))
                {
                    var element = XElement.Parse(objAsString);
                    element.Name = name;
                    return element;
                }
            }
            catch
            {
            }
            return new XElement(name, new XText(obj.ToString()));
        }

        //public string SerializeOld(object message)
        //{
        //    string serializedMessage;
        //    var escapeCharacters = new Dictionary<string, string>
        //                               {
        //                                   {"\"", "&quot"},
        //                                   {"<", "&lt;"},
        //                                   {">", "&gt;"},
        //                                   {"&", "&amp;"}
        //                               };

        //    if (message != null && message is string)
        //    {
        //        serializedMessage = message.ToString();
        //        escapeCharacters.ToList().ForEach(ec =>
        //                                          serializedMessage = serializedMessage.Replace(ec.Key, ec.Value));
        //    }
        //    else if (message is Exception)
        //    {
        //        serializedMessage = ((Exception)message).ToXmlString();
        //    }
        //    else if (message != null && message.GetType().Name.Contains("AnonymousType"))
        //    {
        //        var dic = ToDictionary(message);
        //        if (dic.ContainsKey("Exception") 
        //            && dic["Exception"] != null 
        //            && dic["Exception"] is Exception)
        //        {
        //            dic["Exception"] = ((Exception)dic["Exception"]).ToXmlString();
        //        }
        //        serializedMessage = _serializer.Serialize(dic);
        //    }
        //    else
        //        serializedMessage = _serializer.Serialize(message);

        //    var retVal = string.Format("<Message>{0}</Message>", serializedMessage);
        //    return retVal;
        //}

        // TODO: Add to settings.
        private const int MaxByteArrayLength = 2 * 1024 * 1024;

        private static Dictionary<string, object> ToDictionary(object message)
        {
            var dic = new Dictionary<string, object>();
            if (message == null || message.GetType() == typeof(CultureInfo) || message.GetType() == typeof(Assembly) || message.GetType() == typeof(Type)) return dic;
            
            var props = message.GetType().GetProperties().ToList();
            props.ForEach(p =>
                              {
                                  var val = p.GetValue(message, null);
                                  if (val != null && val.GetType() == typeof (byte[]))
                                  {
                                      dic.Add(p.Name, ((byte[]) val).Length <= MaxByteArrayLength
                                          ? Convert.ToBase64String(((byte[]) val))
                                          : Convert.ToBase64String((new byte[] {})));
                                  }
                                  else if (val != null && val.GetType().Name.Contains("AnonymousType"))
                                      dic.Add(p.Name, ToDictionary(val));
                                  else
                                      dic.Add(p.Name,
                                          p.PropertyType.GetInterface(typeof (IEnumerable).Name) != null &&
                                          p.PropertyType != typeof (string)
                                              ? ToDictionary(p.Name, (IEnumerable) val)
                                              : (p.PropertyType.IsClass && p.PropertyType != typeof (string) &&
                                                 p.PropertyType != typeof (Exception) &&
                                                 p.PropertyType != typeof (object)
                                                  ? ToDictionary(val)
                                                  : val));
                              }
                );
            
            return dic;
        }

        

        private static Dictionary<string, object> ToDictionary(string propertyName, IEnumerable message)
        {
            var dic = new Dictionary<string, object>();
            if (message == null) return dic;
            var index = 0;
            var objects = message.Cast<object>().Select(
                obj => new
                           {
                               Index = propertyName + "_" + (index++).ToString().PadLeft(5, '0'),
                               Object = obj ?? "NULL"
                           })
                .ToList();


            objects.ForEach(
                item =>
                dic.Add(item.Index.ToString(),
                        item.Object.GetType().GetInterface(typeof(IEnumerable).Name) != null && item.Object.GetType() != typeof(string)
                            ? ToDictionary(item.Index, (IEnumerable) item.Object)
                            : (item.Object.GetType().IsClass && item.Object.GetType() != typeof(string) ? ToDictionary(item.Object) : item.Object)));
            
            return dic;
        }
        #endregion
    }
}