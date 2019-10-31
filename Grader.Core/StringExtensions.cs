using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grader.Web
{
    public class IndexedItem<T>
    {
        public T Model { get; set; }
        public int Index { get; set; }
    }
    public static class Extensions
    {
        public static IEnumerable<IndexedItem<T>> ToIndexedItem<T>(this IEnumerable<T> data)
        {
            var dataArray = data.ToList();
            for (var i = 0; i < dataArray.Count; i++)
            {
                yield return new IndexedItem<T>(){Index = i,Model = dataArray[i]};
            }
        }

        public static string RemoveWhiteSpace(this string str)
        {
            var result = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var charItem = str[i];
                if (char.IsWhiteSpace(charItem) != true)
                {
                    result.Append(charItem);
                }
            }

            return result.ToString();
        }
    }
}