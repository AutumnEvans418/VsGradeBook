namespace Grader
{
    public class Class
    {
        public string Id { get; set; }
        public int TeacherId { get; set; }
        public virtual Person Teacher { get; set; }
    }
}