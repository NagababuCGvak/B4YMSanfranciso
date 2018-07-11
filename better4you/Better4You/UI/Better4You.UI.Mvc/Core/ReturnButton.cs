namespace Better4You.UI.Mvc.Core
{
    public class ReturnButton : ButtonBase<ReturnButton>
    {
        public ReturnButton():
            base("", "a")
        {
            //Builder.SetInnerText("Go Back");
            Builder.InnerHtml = "Go Back";
            AddCssClass("btn")
                //.AddCssClass("btn-danger")
                .AddAttribute("onclick", "history.go(-1);return false;")
                .AddAttribute("href", "#");
        }
    }
}