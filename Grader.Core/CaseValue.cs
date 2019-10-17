namespace Grader
{
    public class CaseValue
    {
        public CaseValue(string str)
        {
            var resultContent = "";
            Negate = str.StartsWith("!");
            if (Negate)
            {
                resultContent = str.Substring(1);
            }
            else
            {
                resultContent = str;
            }

            var beg = resultContent.IndexOf('[') ;
            if (beg >= 0)
            {
                var end = resultContent.IndexOf(']');
                Hint = resultContent.Substring(beg +1 , end - beg -1 );
                
                resultContent = resultContent.Substring(0, beg);
            }

            ValueToMatch = resultContent;

        }
        public string Hint { get; set; }
        public string ValueToMatch { get; set; }
        public bool Negate { get; set; }

        public static implicit operator string(CaseValue value)
        {
            return value.ValueToMatch;
        }
        public static implicit operator CaseValue(string str)
        {
            return new CaseValue(str);
        }
    }
}