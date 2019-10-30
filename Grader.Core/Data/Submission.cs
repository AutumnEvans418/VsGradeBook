using System;
using System.Collections.Generic;
using AsyncToolWindowSample;

namespace Grader
{
    public class Submission : BindableBase
    {
        private double _grade;
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string StudentName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public bool IsPlagiarized { get; set; }
        public Submission()
        {
            DateCreated = DateTimeOffset.Now;
            SubmissionFiles = new List<SubmissionFile>();
        }
        public double Grade
        {
            get => _grade;
            set => SetProperty(ref _grade,value);
        }

        public virtual IList<SubmissionFile> SubmissionFiles { get; set; }
    }
}