using System;
using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class ParserException : Exception
    {
        public ParserException() 
        {
            
        }

        public ParserException(string msg) : base(msg)
        {
            
        }
        public List<Exception> Exceptions { get; set; } = new List<Exception>();

        public override string ToString()
        {
            return $"{(Exceptions.Any() ? Exceptions.Select(p=> p.ToString()).Aggregate((f,s) => f + "\r\n" + s) : "")}\r\n" + base.ToString();
        }
    }
    public class GraderParser
    {
        public IList<string> ParseInput(string input)
        {
            var result = Parse(input);
            return result.Select(p=>p.ValueToMatch).ToList();
        }

        public IList<CaseValue> ParseOutput(string output)
        {
            return Parse(output);
          
        }


        

        private IList<CaseValue> Parse(string input)
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

           

            return CheckSyntax(lst);
        }

        private List<CaseValue> CheckSyntax(IEnumerable<Token> lst)
        {
            var values = new List<CaseValue>();
            var data = lst.ToList();
            var token = data.FirstOrDefault();

            void Eat(TokenType tokenType)
            {
                if (data[0].TokenType == tokenType)
                {
                    data.RemoveAt(0);
                    token = data.FirstOrDefault();
                }
                else
                {
                    throw new ParserException($"Expected type '{tokenType}' but was '{data[0]}'. Please check your syntax");
                }
            }

            while (data.Count > 0)
            {
                var val = new CaseValue();
                if (token?.TokenType == TokenType.Bang)
                {
                    val.Negate = true;
                    Eat(TokenType.Bang);
                }

                val.ValueToMatch = token?.Value;
                Eat(TokenType.Id);
                if (token?.TokenType == TokenType.Comment)
                {
                    val.Hint = token.Value;
                    Eat(TokenType.Comment);
                }
                //delete trailing spaces
                if (token?.TokenType == TokenType.Id && string.IsNullOrWhiteSpace(token.Value))
                {
                    Eat(TokenType.Id);
                }
                if (token != null && token?.TokenType != TokenType.EndOfFile)
                {
                    Eat(TokenType.Comma);
                }
                else if(token != null)
                {
                    Eat(TokenType.EndOfFile);
                }
                values.Add(val);

            }


            return values;
        }
    }
}