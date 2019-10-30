using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Grader.Web
{
    public class PlagiarismChecker
    {
        private readonly IGradeBookRepository _repository;

        public PlagiarismChecker(IGradeBookRepository repository)
        {
            _repository = repository;
        }

        public async Task Check(int projectId)
        {
            var submissions = await _repository.GetSubmissions(projectId);

            var combinations = Combinations(submissions, p=>p.ProjectId);

            foreach (var (r, t) in combinations)
            {
                if (r.SubmissionFiles.Any(p => t.SubmissionFiles.Select(s => s.Content).Contains(p.Content)))
                {
                    await _repository.Plagiarized(r, t);
                }
                else if (t.SubmissionFiles.Any(p => r.SubmissionFiles.Select(s => s.Content).Contains(p.Content)))
                {
                    await _repository.Plagiarized(r, t);
                }
            }
        }

        private static IEnumerable<(T r, T t)> Combinations<T,TKey>(IEnumerable<T> submissions, Func<T,TKey> selector) where TKey : IComparable
        {
            var list1 = submissions.ToList();

            var list2 = submissions.ToList();

            var combinations = (from r in list1 from t in list2 where selector(r).CompareTo(selector(r)) == 1 select (r, t));
            return combinations;
        }
    }
}