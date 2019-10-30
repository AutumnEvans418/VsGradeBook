using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Newtonsoft.Json;

namespace Grader
{
    public class CodeProjectValidation : AbstractValidator<CodeProject>
    {
        public CodeProjectValidation()
        {
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
        }
    }

    public class SubmissionValidation : AbstractValidator<Submission>
    {
        public SubmissionValidation()
        {
            RuleFor(p => p.StudentName).NotEmpty();
            RuleFor(p => p.SubmissionFiles).NotEmpty();
        }
    }


    public class GradeBookRepositoryHttpClient : IGradeBookRepository
    {
        private readonly HttpClient _client;
        public const string ApiKey = "bd8d2769-0b9f-404e-a265-9869c8f945b9";
        readonly CodeProjectValidation codeProjectValidation = new CodeProjectValidation();
        readonly SubmissionValidation submissionValidation = new SubmissionValidation();
        public GradeBookRepositoryHttpClient(HttpClient client)
        {
            _client = client;
        }
     
        public async Task<CodeProject> GetCodeProject(Guid? studentCode, Guid? teacherCode)
        {
            var url = "api/projects/project?";
            if (studentCode != null)
            {
                url += "studentCode=" + studentCode;
            }
            else
            {
                url += "teacherCode=" + teacherCode;
            }
            var str = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<CodeProject>(str);
        }

        public async Task<CodeProject> AddProject(CodeProject project)
        {
            codeProjectValidation.ValidateAndThrow(project);
            var result = await _client.PostAsync($"api/projects", new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CodeProject>(content);
        }

        public async Task<Submission> AddSubmission(Submission submission)
        {
            submissionValidation.ValidateAndThrow(submission);
            var result = await _client.PostAsync($"api/submissions", new StringContent(JsonConvert.SerializeObject(submission), Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Submission>(content);
        }

        public async Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode)
        {
            var str = await _client.GetStringAsync($"api/submissions?teacherCode={teacherCode}");
            return JsonConvert.DeserializeObject<IEnumerable<Submission>>(str);
        }

        public async Task<IEnumerable<CodeProject>> GetCodeProjects()
        {
            var url = $"api/projects?apiKey={ApiKey}";
            var str = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<IEnumerable<CodeProject>>(str);
        }

        public Task<IEnumerable<Submission>> GetSubmissions(int projectId)
        {
            throw new NotImplementedException();
        }

        public Task Plagiarized(Submission submission, Submission submission1)
        {
            throw new NotImplementedException();
        }
    }
}