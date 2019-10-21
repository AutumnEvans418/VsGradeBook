namespace Grader
{
    public class Token
    {
        public Token()
        {
            
        }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }
        public string Value { get; set; }
        public TokenType TokenType { get; set; }
    }
}