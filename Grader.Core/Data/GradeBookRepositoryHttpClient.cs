using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Grader
{
    public class GradeBookRepositoryHttpClient : IGradeBookRepository
    {
        private readonly HttpClient _client;
        public const string ApiKey = "bd8d2769-0b9f-404e-a265-9869c8f945b9";

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
            var result = await _client.PostAsync($"api/projects", new StringContent(JsonConvert.SerializeObject(project), Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CodeProject>(content);
        }

        public async Task<Submission> AddSubmission(Submission submission)
        {
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
    }
}