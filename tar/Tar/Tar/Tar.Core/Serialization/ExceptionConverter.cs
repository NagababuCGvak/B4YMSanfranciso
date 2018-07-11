using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Tar.Core.Serialization
{
    public class ExceptionConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var ex = obj as Exception;

            var dictionary = new Dictionary<string, object>();

            if (ex == null) return dictionary;

            var exType = ex.GetType();
            dictionary.Add("ExceptionType", exType.Name);
            dictionary.Add("ExceptionAssembly", exType.Assembly.FullName);

            exType.GetProperties().ToList().ForEach(p => dictionary.Add(p.Name, p.GetValue(ex, null)));

            dictionary.Add("Copyright", "Zahir.Library.Common");

            return dictionary;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof (Exception)}; }
        }
    }
}