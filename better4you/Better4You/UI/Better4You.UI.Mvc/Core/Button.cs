namespace Better4You.UI.Mvc.Core
{
    public class Button : ButtonBase<Button>
    {
        public Button(string id, string text) :
            base(id, "button")
        {
            Builder.SetInnerText(text);
        }
    }
}