using System;
using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GraderParser
    {
        public IList<string> ParseInput(string input)
        {
            var result = Parse(input);
            return result.Where(p => p.TokenType == TokenType.Id).Select(p => p.Value).ToList();
        }

        public IList<CaseValue> ParseOutput(string output)
        {
            var result = Parse(output);
            var lst = new List<CaseValue>();


            var index = 0;
            Token get(int i)
            {
                if (i < result.Count && i >= 0)
                {
                    return result[i];
                }
                return null;
            }
            while (index < result.Count)
            {
                var c = get(index);

                if (c.TokenType == TokenType.Id)
                {
                    var current = new CaseValue();
                    current.ValueToMatch = c.Value;
                    if (get(index - 1)?.TokenType == TokenType.Bang)
                    {
                        current.Negate = true;
                    }

                    var cmt = (get(index + 1));
                    if (cmt?.TokenType == TokenType.Comment)
                    {
                        current.Hint = cmt.Value;
                    }
                    lst.Add(current);
                }
                index++;


            }

            return lst;
        }


        void Assert(TokenType tokenType, TokenType expected)
        {
            if (tokenType != expected)
            {
                throw new InvalidOperationException($"Unexpected token {tokenType}");
            }
        }



        public IList<Token> Parse(string input)
        {
            var lst = new List<Token>();

            var index = 0;
            //var current = "";

            char? get()
            {
                if (index < input.Length)
                {
                    return input[index];
                }

                return null;
            }

            void Add(TokenType type, string val, bool increment = true)
            {
                lst.Add(new Token() { TokenType = type, Value = val });
                if (increment)
                    index++;
            }

            while (index < input.Length)
            {
                var c = get();
                if (c == '!')
                {
                    Add(TokenType.Bang, "!");
                }
                else if (c == '"')
                {
                    var current = "";
                    index++;
                    c = get();

                    while (c != '"')
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    Add(TokenType.Id, current);
                }
                else if (c == '[')
                {
                    var current = "";
                    index++;
                    c = get();

                    while (c != ']')
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    Add(TokenType.Comment, current);
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