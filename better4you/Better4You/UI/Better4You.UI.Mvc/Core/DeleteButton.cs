namespace Better4You.UI.Mvc.Core
{
    public class DeleteButton : MvcControl<DeleteButton>
    {
        public DeleteButton(string action, string controller, string id)
            : base("a")
        {
            Builder.InnerHtml = "<i class='icon-remove icon-white'></i>&nbsp;Delete";
            AddCssClass("btn")
                .AddCssClass("btn-danger")
                .AddAttribute("onclick", "tar.goTo('" + action + "','" + controller + "',{Id:" + id + "});")
                .AddAttribute("href", "#");
        }
    }
}