using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GraderParser
    {
        private int index;
        private List<Token> lst;
        void Add(TokenType type, string val, bool increment = true)
        {
            lst.Add(new Token() { TokenType = type, Value = val });
            if (increment)
            {
                index++;
            }
        }
        public IList<Token> Parse(string input)
        {
            lst = new List<Token>();
            index = 0;
            char? get(int? i = null)
            {
                if (i == null)
                {
                    i = index;
                }
                if (i < input.Length)
                {
                    return input[(int)i];
                }

                return null;
            }
            
            while (index < input.Length)
            {
                var c = get();
                bool ParseTill(char from, char to, TokenType type)
                {
                    if (c == from)
                    {
                        var current = "";
                        index++;
                        c = get();

                        while (c != to)
                        {
                            current += c;
                            index++;
                            c = get();
                            if (c == null)
                            {
                                throw new ParserException($"Expected a '{to}' but was the end of the text");
                            }
                        }
                        Add(type, current);
                        return true;
                    }
                    return false;
                }

                if (c == '!')
                {
                    Add(TokenType.Bang, "!");
                }
                else if (c == '^' && get(index + 1) == 'i')
                {
                    Add(TokenType.CaseInsensitive, "^i");
                    index++;
                }
                else if (c == '^' && get(index + 1) == 'e' && get(index + 2) == 'x')
                {
                    Add(TokenType.Exception, "^ex");
                    index += 2;
                }
                else if (c == '^' && get(index + 1) == 'e' && get(index + 2) == 'q')
                {
                    Add(TokenType.Equal, "^eq");
                    index += 2;
                }
                else if(c == '^' && char.IsDigit(get(index + 1) ?? ' '))
                {
                    index++;
                    c = get();
                    var current = "^";
                    while (c is char car && char.IsDigit(car))
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    Add(TokenType.Order, current, false);
                }
                else if (c == '^' && get(index + 1) == 'r')
                {
                    Add(TokenType.Regex, "^r");
                    index++;
                    var current = "";
                    c = get();

                    while (c != null)
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    Add(TokenType.Id, current);
                }
                else if (ParseTill('"','"', TokenType.Id))
                {
                }
                else if (ParseTill('[',']', TokenType.Comment))
                {
                }
                else if (c == ',')
                {
                    Add(TokenType.Comma, "");
                }
                else if (c == null)
                {
                    Add(TokenType.EndOfFile, "");
                }
                else
                {
                    var stops = new char?[] { ',', '"', '[', null }.ToList();
                    var current = "";
                    while (stops.Contains(c) != true)
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    if(c == '"' && string.IsNullOrWhiteSpace(current))
                    {
                    }
                    else
                    {
                        Add(TokenType.Id, current, false);
                    }

                }

            }

            return lst;
        }

    }
}