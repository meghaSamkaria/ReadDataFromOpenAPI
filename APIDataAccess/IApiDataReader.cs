namespace StarWarsOpenAPI.APIDataAccess
{
    public interface IApiDataReader
    {
        Task<string> Read(string baseaddr, string restOfAPI);
    }
}