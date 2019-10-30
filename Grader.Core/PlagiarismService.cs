using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grader.Web
{
    public class PlagiarismService : IPlagiarismService
    {
        private readonly IGradeBookRepository _repository;

        public PlagiarismService(IGradeBookRepository repository)
        {
            _repository = repository;
        }

        public async Task Check(int projectId)
        {
            var submissions = await _repository.GetSubmissions(projectId);

            var combinations = Combinations(submissions);
            var plagiarized = new List<Submission>();
            foreach (var (r, t) in combinations)
            {
                if (r.SubmissionFiles.Any(p => t.SubmissionFiles.Select(s => s.Content).Contains(p.Content)))
                {
                    plagiarized.Add(r);
                    plagiarized.Add(t);
                }
                else if (t.SubmissionFiles.Any(p => r.SubmissionFiles.Select(s => s.Content).Contains(p.Content)))
                {
                    plagiarized.Add(r);
                    plagiarized.Add(t);
                }
            }

            await _repository.Plagiarized(plagiarized);

        }

        private static IEnumerable<(Submission r, Submission t)> Combinations(IEnumerable<Submission> submissions) 
        {
            var list1 = submissions.ToList();

            var list2 = submissions.ToList();

            var combinations = (from r in list1 from t in list2 orderby r.ProjectId where r.ProjectId > t.ProjectId select (r, t)).ToList();
            return combinations;
        }
    }
}