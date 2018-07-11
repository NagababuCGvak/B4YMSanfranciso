using System.Runtime.Serialization;

namespace Tar.ViewModel
{
    [DataContract]
    public class GeneralItemView:IView
    {
        [DataMember]
        //public int Id { get; set; }
        public long Id { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Text { get; set; }

        public GeneralItemView()
        {
        }
        public GeneralItemView(long id, string value, string text)
        //public GeneralItemView(int id, string value, string text)
        {
            Id = id;
            Text = text;
            Value = value;
        }
    }
}