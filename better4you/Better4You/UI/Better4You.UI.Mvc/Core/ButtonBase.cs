namespace Better4You.UI.Mvc.Core
{
    public abstract class ButtonBase<T> : MvcControl<T> where T:ButtonBase<T>
    {
        protected ButtonBase(string id,string tagName) : base(tagName)
        {
            AddCssClass("btn").
                AddAttribute("id", id).
                AddAttribute("name", id);
        }

        public T Primary()
        {
            return AddCssClass("btn-primary");
        }
        public T Info()
        {
            return AddCssClass("btn-info");
        }
        public T Success()
        {
            return AddCssClass("btn-success");
        }
        public T Warning()
        {
            return AddCssClass("btn-warning");
        }
        public T Danger()
        {
            return AddCssClass("btn-danger");
        }
        public T Inverse()
        {
            return AddCssClass("btn-inverse");
        }
        public T Link()
        {
            return AddCssClass("btn-link");
        }

        public T Large()
        {
            return AddCssClass("btn-large");
        }
    }
}