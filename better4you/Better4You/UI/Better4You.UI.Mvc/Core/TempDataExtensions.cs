using System.Web.Mvc;

namespace Better4You.UI.Mvc.Core
{
    public static class TempDataExtensions
    {
        public static string ErrorMessage(this TempDataDictionary tempData)
        {
            return (string)tempData["ErrorMessage"];
        }

        public static void ErrorMessage(this TempDataDictionary tempData, string errorMessage)
        {
            tempData["ErrorMessage"] = errorMessage;
        }

        public static string InfoMessage(this TempDataDictionary tempData)
        {
            return (string)tempData["InfoMessage"];
        }

        public static void InfoMessage(this TempDataDictionary tempData, string errorMessage)
        {
            tempData["InfoMessage"] = errorMessage;
        }
    }
}