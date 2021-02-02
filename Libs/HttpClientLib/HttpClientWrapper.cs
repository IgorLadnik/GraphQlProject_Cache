using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpClientLib
{
    public class HttpClientWrapper
    {
        public readonly HttpClient _client;

        public bool IsOK { get; private set; } = true;
        public string ErrorMessage { get; private set; }

        public HttpClientWrapper(HttpClient client = null, string token = null)
        {
            _client = client == null ? new() : client;
            AssignToken(token);
        }

        public async Task<string> LoginAsync(string uri, string userName, string password)
        {
            var str = "{'username':/*username*/,'password':/*password*/}"
                .Replace("/*username*/", $"'{userName}'")
                .Replace("/*password*/", $"'{password}'")
                .Replace("'", "\"");
            var token = await PostInnerAsync(uri, str);
            if (!IsOK)
                return null;

            AssignToken(token);
            return token;
        }

        public async Task<string> PostAsync(string uri, string query)
        {
            var postData = new { Query = query };
            return await PostInnerAsync(uri, JsonConvert.SerializeObject(postData));
        }

        private async Task<string> PostInnerAsync(string uri, string serializedPostData = null)
        {
            ErrorMessage = null;
            StringContent stringContent = !string.IsNullOrEmpty(serializedPostData) 
                ? new(serializedPostData, Encoding.UTF8, "application/json")
                : null;
            var response = await _client.PostAsync(uri, stringContent);
            return (IsOK = response.IsSuccessStatusCode)
                ? await response.Content.ReadAsStringAsync()
                : ErrorMessage = $"ERROR. Status code:{response.StatusCode}";
        }

        private void AssignToken(string token) 
        {
            if (_client != null  && !string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
