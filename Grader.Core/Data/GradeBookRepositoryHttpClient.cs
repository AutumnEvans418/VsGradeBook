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
    }
}