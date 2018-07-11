namespace Better4You.Core
{
    public class GeneralItemView
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public GeneralItemView()
        {
        }
        public GeneralItemView(string value,string text)
        {
            Value = value;
            Text = text;
        }
    }

//    public static class GeneralItemViewExtensions
//    {
//        public static GeneralItemView ToGeneralItemView<T>(this T entity, Func<T, int> valueSelector, Func<T, string> textSelector) where T : IEntity
//        {
//            return new GeneralItemView(
//                valueSelector(entity).ToString(CultureInfo.InvariantCulture),
//                textSelector(entity));
//        }

//        public static IList<GeneralItemView> ToGeneralItemViewList<T>(this IEnumerable<T> entityList, Func<T, int> valueSelector, Func<T, string> textSelector)
//            where T : IEntity
//        {
//            return entityList.Select(e => e.ToGeneralItemView(valueSelector, textSelector)).ToList();
//        }
//    }
}