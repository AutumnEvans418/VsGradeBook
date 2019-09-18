using System.Collections.Generic;

namespace Grader
{
    public class Class
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int TeacherId { get; set; }
        public virtual Person Teacher { get; set; }
        public virtual IList<Enrollment> Enrollments { get; set; }
    }
}