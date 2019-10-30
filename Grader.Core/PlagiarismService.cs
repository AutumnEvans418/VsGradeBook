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
            foreach (var (first, second) in combinations)
            {
                if (first.SubmissionFiles.Any(p => 
                    second.SubmissionFiles.Select(s => s.Content.RemoveWhiteSpace()).Contains(p.Content.RemoveWhiteSpace())))
                {
                    plagiarized.Add(first);
                    plagiarized.Add(second);
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