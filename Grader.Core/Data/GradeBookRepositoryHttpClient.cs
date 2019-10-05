using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Grader
{
    public class GradeBookRepositoryHttpClient : IGradeBookRepository
    {
        private readonly HttpClient _client;

        public GradeBookRepositoryHttpClient(HttpClient client)
        {
            _client = client;
        }
        public async Task<int> GetPersonId(string userName)
        {
            var id = await _client.GetStringAsync($"api/People?userName={userName}");
            return JsonConvert.DeserializeObject<int>(id);
        }

        public async Task<Person> AddPerson(Person person)
        {
            var result = await _client.PostAsync($"api/People", new StringContent(JsonConvert.SerializeObject(person)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Person>(content);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var result = await _client.PutAsync($"api/People", new StringContent(JsonConvert.SerializeObject(person)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Person>(content);
        }

        public async Task DeletePerson(int person)
        {
            var result = await _client.DeleteAsync($"api/People?personId={person}");
            
        }

        public async Task<IEnumerable<Class>> GetClasses(int personId)
        {
            var classes = await _client.GetStringAsync($"api/Classes?personId={personId}");
            return JsonConvert.DeserializeObject<List<Class>>(classes);
        }

        public async Task<Class> UpdateClass(Class cClass)
        {
            var result = await _client.PutAsync($"api/Classes", new StringContent(JsonConvert.SerializeObject(cClass)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Class>(content);
        }

        public async Task<Class> AddClass(Class cClass)
        {
            var result = await _client.PostAsync($"api/Classes", new StringContent(JsonConvert.SerializeObject(cClass)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Class>(content);
        }

        public async Task DeleteClass(string id)
        {
            var result = await _client.DeleteAsync($"api/Classes?classId={id}");

        }

        public async Task<IEnumerable<Enrollment>> GetEnrollments()
        {
            var str = await _client.GetStringAsync($"api/enrollments");
            return JsonConvert.DeserializeObject<IEnumerable<Enrollment>>(str);
        }

        public async Task<Enrollment> GetEnrollment(int enrollmentId)
        {
            var str = await _client.GetStringAsync($"api/enrollments/{enrollmentId}");
            return JsonConvert.DeserializeObject<Enrollment>(str);
        }

        public async Task<Enrollment> AddEnrollment(Enrollment enrollment)
        {
            var result = await _client.PostAsync($"api/enrollments", new StringContent(JsonConvert.SerializeObject(enrollment)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Enrollment>(content);
        }

        public async Task<Enrollment> UpdateEnrollment(Enrollment enroll)
        {
            var result = await _client.PutAsync($"api/enrollments/{enroll.Id}", new StringContent(JsonConvert.SerializeObject(enroll)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Enrollment>(content);
        }

        public async Task DeleteEnrollment(int enrollmentId)
        {
            var result = await _client.DeleteAsync($"api/enrollments/{enrollmentId}");
        }

        public async Task<CodeProject> GetCodeProject(Guid studentCode)
        {
            var str = await _client.GetStringAsync($"api/project?studentCode={studentCode}");
            return JsonConvert.DeserializeObject<CodeProject>(str);
        }

        public async Task<CodeProject> AddProject(CodeProject project)
        {
            var result = await _client.PostAsync($"api/project", new StringContent(JsonConvert.SerializeObject(project)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CodeProject>(content);
        }

        public async Task<Submission> AddSubmission(Submission submission)
        {
            var result = await _client.PostAsync($"api/submission", new StringContent(JsonConvert.SerializeObject(submission)));

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Submission>(content);
        }

        public async Task<IEnumerable<Submission>> GetSubmissions(Guid teacherCode)
        {
            var str = await _client.GetStringAsync($"api/submission?teacherCode={teacherCode}");
            return JsonConvert.DeserializeObject<IEnumerable<Submission>>(str);
        }
    }
}