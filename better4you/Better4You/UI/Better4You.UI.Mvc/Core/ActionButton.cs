namespace Better4You.UI.Mvc.Core
{
    public class ActionButton : ButtonBase<ActionButton>
    {
        public ActionButton(string text, string action, string controller, string id)
            : base("","a")
        {
            Builder.InnerHtml = //"<i class='icon-white'></i>" + 
                text;
            AddCssClass("btn")
                //.AddCssClass("btn-danger")
                .AddAttribute("onclick", "tar.goTo('" + action + "','" + controller + (string.IsNullOrEmpty(id)?"{}": "',{Id:" + id + "});"))
                .AddAttribute("href", "#");
        }
    }
}