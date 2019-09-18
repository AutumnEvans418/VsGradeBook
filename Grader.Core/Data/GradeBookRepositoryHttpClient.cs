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

        public async Task<IEnumerable<Class>> GetClasses(int personId)
        {
            var classes = await _client.GetStringAsync($"api/Classes?personId={personId}");
            return JsonConvert.DeserializeObject<List<Class>>(classes);
        }
    }
}