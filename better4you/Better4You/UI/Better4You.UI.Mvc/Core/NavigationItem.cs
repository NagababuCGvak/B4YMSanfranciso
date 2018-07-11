namespace Better4You.UI.Mvc.Core
{
    public class NavigationItem
    {

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string[] Actions { get; set; }
        public string IconClass { get; set; }
        public string Title { get; set; }
    }
}