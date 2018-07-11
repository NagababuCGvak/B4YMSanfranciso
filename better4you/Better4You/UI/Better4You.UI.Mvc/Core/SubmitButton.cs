namespace Better4You.UI.Mvc.Core
{
    public class SubmitButton : ButtonBase<SubmitButton>
    {
        public SubmitButton(string id,string value = null) :
            base(id,"input")
        {
            if (!string.IsNullOrEmpty(value))
                AddAttribute("value", value);
            AddAttribute("type", "submit");
        }
    }
}