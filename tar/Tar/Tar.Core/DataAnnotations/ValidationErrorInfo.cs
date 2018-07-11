using System;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// Error Info
    /// </summary>
    public class ValidationErrorInfo
    {
        /// <summary>
        /// PropertyName
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// FormatErrorMessage
        /// </summary>
        public string FormatErrorMessage { get; set; }

        /// <summary>
        /// Instance
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Error Info Constructor
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="formatErrorMessage"></param>
        /// <param name="instance"></param>
        public ValidationErrorInfo(string propertyName, string formatErrorMessage, object instance)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (formatErrorMessage == null) throw new ArgumentNullException("formatErrorMessage");
            if (instance == null) throw new ArgumentNullException("instance");

            PropertyName = propertyName;
            FormatErrorMessage = formatErrorMessage;
            Instance = instance;
        }
    }
}