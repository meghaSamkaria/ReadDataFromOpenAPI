namespace StarWarsOpenAPI.APIDataAccess
{
    public class ApiDataReader : IApiDataReader
    {
        public async Task<string> Read(string baseaddr, string restOfAPI)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseaddr);
            var response = await client.GetAsync(restOfAPI);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var rString = await response.Content.ReadAsStringAsync();
                    return rString;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex.Message}";
            }
        }
    }
}