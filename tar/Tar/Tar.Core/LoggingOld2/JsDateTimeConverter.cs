using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Tar.Core.LoggingOld2
{
    class JsDateTimeConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        /// <summary>
        /// When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        /// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2"/> instance of property data stored as name/value pairs.
        ///                 </param><param name="type">The type of the resulting object.
        ///                 </param><param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer"/> instance.
        ///                 </param>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return DateTime.Parse(dictionary["DateTime"].ToString());
        }

        /// <summary>
        /// When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <returns>
        /// An object that contains key/value pairs that represent the object�s data.
        /// </returns>
        /// <param name="obj">The object to serialize.
        ///                 </param><param name="serializer">The object that is responsible for the serialization.
        ///                 </param>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var r = new Dictionary<string, object>
                        {
                            {"DateTime", obj.ToString()}
                        };
            return r;
        }

        /// <summary>
        /// When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        /// <returns>
        /// An object that implements <see cref="T:System.Collections.Generic.IEnumerable`1"/> that represents the types supported by the converter.
        /// </returns>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new List<Type> { typeof(DateTime) }; }
        }

        #endregion
    }
}