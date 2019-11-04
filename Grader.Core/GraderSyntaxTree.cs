using System.Collections.Generic;
using System.Linq;

namespace Grader
{
    public class GraderSyntaxTree
    {
        public List<CaseValue> CheckSyntax(IEnumerable<Token> lst)
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
                while (token != null && token.TokenType != TokenType.Id)
                {
                    if (token?.TokenType == TokenType.Bang)
                    {
                        val.Negate = true;
                        Eat(TokenType.Bang);
                    }
                    else if (token?.TokenType == TokenType.CaseInsensitive)
                    {
                        val.CaseInsensitive = true;
                        Eat(TokenType.CaseInsensitive);
                    }
                    else if (token?.TokenType == TokenType.Exception)
                    {
                        val.Exception = true;
                        Eat(TokenType.Exception);
                    }
                    else if (token?.TokenType == TokenType.Equal)
                    {
                        val.ExactMatch = true;
                        Eat(TokenType.Equal);
                    }
                    else if (token?.TokenType == TokenType.Order)
                    {
                        val.MatchOutputIndex = int.Parse(token.Value.Substring(1));
                        Eat(TokenType.Order);
                    }
                    else if (token?.TokenType == TokenType.Regex)
                    {
                        val.Regex = true;
                        Eat(TokenType.Regex);
                        break;
                    }
                    else
                    {
                        throw new ParserException($"Did not recognize token {token}");
                    }
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
                else if (token != null)
                {
                    Eat(TokenType.EndOfFile);
                }
                values.Add(val);

            }


            return values;
        }

    }
}