namespace Better4You.UI.Mvc.Filters
{
    public class CancelButtonAttribute : ButtonAttribute
    {
        public CancelButtonAttribute()
            : base("action.cancel", "isCancel")
        {
        }
    }
}