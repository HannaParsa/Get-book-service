namespace taaghche.Services
{
    public class TaghcheService : ITaghcheService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TaghcheService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetBookDataAsync(int bookId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            string url = $"https://get.taaghche.com/v2/book/{bookId}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
