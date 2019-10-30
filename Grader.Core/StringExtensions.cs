using System.Text;

namespace Grader.Web
{
    public static class StringExtensions
    {
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