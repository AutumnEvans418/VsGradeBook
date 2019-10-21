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
            var lst = new List<string>();
            foreach (var token in result)
            {
                Assert(token.TokenType, TokenType.Id);
                lst.Add(token.Value);
            }
            return lst;
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
            var current = "";

            char? get()
            {
                if (index < input.Length)
                {
                    return input[index];
                }

                return null;
            }
            while (index < input.Length)
            {
                var c = get();
                if (c == '!')
                {
                    lst.Add(new Token() { TokenType = TokenType.Bang, Value = "!" });
                    index++;
                    current = "";
                }
                else if (c == '"')
                {
                    index++;
                    c = get();
                    if (string.IsNullOrWhiteSpace(current))
                    {
                        current = "";
                    }
                    while (c != '"')
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    lst.Add(new Token(TokenType.Id, current));
                    current = "";
                    index++;
                }
                else if (c == '[')
                {
                    index++;
                    c = get();
                    if (string.IsNullOrWhiteSpace(current))
                    {
                        current = "";
                    }
                    while (c != ']')
                    {
                        current += c;
                        index++;
                        c = get();
                    }
                    lst.Add(new Token(TokenType.Id, current));
                    current = "";
                    index++;
                }
                else if (c == ',' || c == null)
                {
                    //lst.Add(new Token(TokenType.Id, current));
                    current = "";
                    index++;
                }
                else
                {
                    current += c;
                    index++;
                }

            }

            if (current != "")
            {
                lst.Add(new Token(TokenType.Id, current));
                current = "";
            }

            return lst;
        }
    }
}