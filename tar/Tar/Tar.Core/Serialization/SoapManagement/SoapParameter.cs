namespace Tar.Core.Serialization.SoapManagement
{
    public class SoapParameter
    {
        public string EndPoint { get; set; }
        public string InputMessageXslt { get; set; }
        public string OutputMessageXslt { get; set; }
        public string SoapAction { get; set; }

        public BindingType BindingType { set; get; }

        public SoapParameter()
        {
            BindingType = BindingType.BasicHttpBinding;
        }
    }
}