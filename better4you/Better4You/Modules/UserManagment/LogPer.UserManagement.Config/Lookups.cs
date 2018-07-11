using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tar.ViewModel;

namespace Better4You.UserManagement.Config
{
    public static class Lookups
    {
        private static readonly Dictionary<Type, List<GeneralItemView>> Source = new Dictionary<Type, List<GeneralItemView>>();

        public static GeneralItemView GetItem<T>(T val)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof(T)].FirstOrDefault(x => x.Id == (long)(object)val);
        }
        public static GeneralItemView GetItem<T>(long val)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof(T)].FirstOrDefault(x => x.Id == (long)(object)val);
        }
        public static List<GeneralItemView> GetItems<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof (T)];
        }

        private static void AddIfNotExist<T>() 
            where T:struct , IComparable, IFormattable, IConvertible
        {
            if (Source.ContainsKey(typeof (T))) return;
            
            var members = typeof(T).GetMembers();
            var list = new List<GeneralItemView>();
            foreach (var enumVal in Enum.GetValues(typeof(T)))
            {
                var text = enumVal.ToString();
                members.First(x => x.Name == text)
                    .GetCustomAttributes(typeof (DescriptionAttribute), true)
                    .Cast<DescriptionAttribute>()
                    .Where(x => !string.IsNullOrEmpty(x.Description))
                    .ToList().ForEach(x => text = x.Description);
                list.Add(new GeneralItemView((int) enumVal, ((int) enumVal).ToString("G"), text));
            }
            Source.Add(typeof (T), list);
        }
    }
}
