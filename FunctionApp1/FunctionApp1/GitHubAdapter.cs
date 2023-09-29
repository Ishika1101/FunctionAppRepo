using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GithubToFileShareApp
{
    public class GithubAdapter : IGithub
    {
        private readonly HttpClient _httpClient;
        private readonly string _personalAccessToken;

        public GithubAdapter()
        {
            _personalAccessToken = "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";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _personalAccessToken);
        }

        public async Task<string> GetCommitDetails(string repositoryOwner, string repositoryName, string commitId)
        {
            string apiUrl = $"https://api.github.com/repos/{repositoryOwner}/{repositoryName}/commits/{commitId}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return responseBody;
            }
            else
            {
                return $"Request failed with status code: {response.StatusCode}";
            }
        }
    }
}
