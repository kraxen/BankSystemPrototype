using System.Collections;
using System.Collections.Generic;

namespace BankSystemPrototype.ApplicationServices.Helper
{
    public static class ExtentionActions
    {
        public static IEnumerable<T> TypeOf<T>(this IEnumerable collection)
        {
            foreach(var item in collection)
            {
                if (item.GetType() == typeof(T)) yield return (T)item;
            }
        }
    }
}
