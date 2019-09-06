using System.Collections.Generic;

namespace Grader
{
    public class CsvParser
    {
        public List<List<string>> Parse(string source)
        {
            var listList = new List<List<string>>();
            var list = new List<string>();
            var i = 0;
            listList.Add(list);
            while (i < source.Length)
            {
                if (i < source.Length && source[i] == '"')
                {
                    i++;
                    var val = "";
                    while (i < source.Length && source[i] != '"')
                    {
                        val += source[i];
                        i++;
                    }
                    i++;
                    list.Add(val);
                }

                var v = "";
                while (i < source.Length && source[i] != ',' && source[i] != '\r' && source[i] != '\n')
                { 
                    v += source[i];
                    i++;
                }

                if (i < source.Length && source[i] == '\r' && source[i] == '\n')
                {
                    
                    list = new List<string>();
                    listList.Add(list);
                    i++;
                    i++;
                }

                if (i < source.Length && source[i] == ',')
                {
                    list.Add(v);
                    i++;
                }
            }

            return listList;

        }
    }
}