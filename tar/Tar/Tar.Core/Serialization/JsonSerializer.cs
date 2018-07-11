using System;
using System.Text;
using System.Web.Script.Serialization;

namespace Tar.Core.Serialization
{
    public class JsonSerializer
    {
        public static JsonSerializer Instance;

        static JsonSerializer()
        {
            Instance = new JsonSerializer();
        }

        private readonly JavaScriptSerializer _javaScriptSerializer;

        public JsonSerializer()
        {
            _javaScriptSerializer = new JavaScriptSerializer();
            RegisterConverter(new ExceptionConverter());
        }

        public string Serialize(object obj)
        {
            if (obj is Exception)
                return
                    _javaScriptSerializer.Serialize(
                        new
                            {
                                Message = ((Exception) obj).Message,
                                StackTrace = ((Exception) obj).StackTrace,
                                String = obj.ToString()
                            });
            return _javaScriptSerializer.Serialize(obj);
        }

        public void Serialize(object obj, StringBuilder output)
        {
            _javaScriptSerializer.Serialize(obj, output);
        }
        public T Deserialize<T>(string input)
        {
            return _javaScriptSerializer.Deserialize<T>(input);
        }

        public void RegisterConverter(JavaScriptConverter javaScriptConverter)
        {
            _javaScriptSerializer.RegisterConverters(new[] {javaScriptConverter});
        }
    }
}
