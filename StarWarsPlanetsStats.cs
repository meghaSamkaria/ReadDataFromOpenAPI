using StarWarsOpenAPI.APIDataAccess;
using StarWarsOpenAPI.DTOs;
using System.Text.Json;

namespace StarWarsOpenAPI
{
    public static class StringExtensions
    {
        public static int? ToIntOrNull(this string? toBeConverted)   //making this an extension method as this has no role in starwarstat class
        {
            if (int.TryParse(toBeConverted, out int parsedType))
            {
                return parsedType;
            }
            return null;
        }
    }
    public class StarWarsPlanetsStats
    {
        private readonly IApiDataReader? _reader;
        private readonly IApiDataReader? _secondaryReader;

        public StarWarsPlanetsStats(ApiDataReader reader, MockStarWarsApiDataReader secondaryReader)
        {
            _reader = reader;
            _secondaryReader = secondaryReader;
        }

        public async Task run()
        {
            string baseaddr = "https://swapi.info/api/";
            string restOfAPI = "planets/";
          
            string? json = null;
            try
            {
                //_reader = new ApiDataReader();
                json = await _reader.Read(baseaddr, restOfAPI);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
               // return;
            }
            if (json is null)
            {
               // _secondaryReader = new MockStarWarsApiDataReader();
                json = await _secondaryReader.Read(baseaddr, restOfAPI);
            }
            var root = JsonSerializer.Deserialize<Root>(json);

            var planets = ToPlanets(root);
           // PrettyPrint(root);
        }

        private IEnumerable<Planet> ToPlanets(Root? root)
        {
            List<Planet> planets = new List<Planet>();
            if (root == null) { 
                throw new ArgumentNullException(nameof(root));
            }
            foreach (var planetDTO in root.results)
            {
                Planet planet = (Planet)planetDTO;
                planets.Add(planet);
            }
            return planets;
        }
        public readonly record struct Planet
        {
            public Planet(string name, int diameter, int? surfaceWater, int? population)
            {
                if(name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Name = name;
                Diameter = diameter;
                SurfaceWater = surfaceWater;
                Population = population;
            }

            public string Name { get;  }
            public int Diameter { get; }
            public int? SurfaceWater {  get; }
            public int? Population { get; }

            public static explicit operator Planet(Results planetDto)
            {
                string name = planetDto.name;
                int dia = Convert.ToInt32(planetDto.diameter);

                // int water = Convert.ToInt32(planetDto.surface_water); //use try parse as we dont want the water to contain anything if value is null

                int? water = planetDto.surface_water.ToIntOrNull();  // notice how this is being called now as its method of string now

                int? population = planetDto.population.ToIntOrNull();

                return new Planet(name, dia, water, population);
            }
        }

        private static void PrettyPrint(Root? root)
        {
            Console.WriteLine($"{"Name",-15}|{"Diameter",-15}|{"Surface Water",-15}|{"Population",-15}|");
            Console.WriteLine("------------------------------------------------------------------");
            foreach (var planet in root.results)
            {
                Console.Write($"{planet.name,-15}|{planet.diameter,-15}|");
                if (planet.surface_water != "unknown")
                {
                    Console.Write($"{planet.surface_water,-15}|");
                }
                else
                {
                    Console.Write($"{"",-15}|");
                }
                if (planet.population != "unknown")
                {
                    Console.WriteLine($"{planet.population,-15}|");
                }
                else
                {
                    Console.WriteLine($"{"",-15}|");
                }
            }
        }
    }
}



