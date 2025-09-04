using StarWarsOpenAPI.APIDataAccess;
using StarWarsOpenAPI.DTOs;
using System.Text.Json;
using static StarWarsOpenAPI.StarWarsPlanetsStats;


namespace StarWarsOpenAPI
{
    public class PlanetFromApiReader : IPlanetsReader
    {
       
        private ApiDataReader apiDataReader;
        private MockStarWarsApiDataReader mockStarWarsApiDataReader;
        private ConsoleUserInteractor consoleUserInteractor;

        public PlanetFromApiReader(ApiDataReader apiDataReader, MockStarWarsApiDataReader mockStarWarsApiDataReader, ConsoleUserInteractor consoleUserInteractor)
        {
            this.apiDataReader = apiDataReader;
            this.mockStarWarsApiDataReader = mockStarWarsApiDataReader;
            this.consoleUserInteractor = consoleUserInteractor;
        }

        public async Task<IEnumerable<Planet>> Read()
        {
            string baseaddr = "https://swapi.info/api/";
            string restOfAPI = "planets/";

            string? json = null;

            /********This is problematic code as the URL is blocked by our IT)
            try
            {     
                 json = await _reader.Read(baseaddr, restOfAPI);
            }
            catch (HttpRequestException e)
            {
                 Console.WriteLine($"Request error: {e.Message}");
            }
            //*************************************************************/

            if (mockStarWarsApiDataReader != null)
            {
                json ??= await mockStarWarsApiDataReader.Read(baseaddr, restOfAPI);
            }

            var root = JsonSerializer.Deserialize<Root>(json);
            return ToPlanets(root);
        }

        private static IEnumerable<Planet> ToPlanets(Root? root)
        {

            if (root == null || root.results == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            return root.results.Select(planetDTO => (Planet)planetDTO);

        }
    }


}



