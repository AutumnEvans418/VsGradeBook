namespace Grader
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string ClassId { get; set; }
        public int StudentId { get; set; }
        public virtual Class Class { get; set; }
        public virtual Person Student { get; set; }
    }
}