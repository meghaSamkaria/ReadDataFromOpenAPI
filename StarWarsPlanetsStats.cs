using StarWarsOpenAPI.APIDataAccess;
using StarWarsOpenAPI.DTOs;
using System.Text.Json;
using static StarWarsOpenAPI.StarWarsPlanetsStats;

namespace StarWarsOpenAPI
{
    public static class StringExtensions
    {
        public static int? ToIntOrNull(this string? toBeConverted)   //making this an extension method as this has NO role in starwarstat class
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

            /********This is problematic code as the URL is blocked by our IT)
            try
            {     
                json = await _reader.Read(baseaddr, restOfAPI);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
           *************************************************************/
            {
                if (json is null)
                {
                    json = await _secondaryReader.Read(baseaddr, restOfAPI);
                }
                   var root = JsonSerializer.Deserialize<Root>(json);
                //var root = JsonSerializer.Deserialize<List<Results>>(json);
                var planets = ToPlanets(root);
            foreach (var planet in planets)
            {
                Console.WriteLine(planet);
            }
                Console.WriteLine();
                Console.WriteLine("The statistics of which property would you like to see?");
                Console.WriteLine("Population");
                Console.WriteLine("diameter");
                Console.WriteLine("surface water");

               
                var userChoice = Console.ReadLine();
               
                if (userChoice == "Population")
                {
                    ShowStatistics(userChoice, planets, planet => planet.Population);
                }
                if (userChoice == "Diamter")
                {
                    ShowStatistics(userChoice, planets, planet => planet.Diameter);
                }
                if (userChoice == "SurfaceWater")
                {
                    ShowStatistics(userChoice, planets, planet => planet.SurfaceWater);
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }

                // PrettyPrint(root);
            }
        }

        private void ShowStatistics(string userChoice, IEnumerable<Planet> planets, Func<Planet, int?> value)
        {
            var maxStat = planets.MaxBy(value);
            var minStat = planets.MinBy(value);
            Console.WriteLine($"max {userChoice} is for planet:" + $"{maxStat.Name} with value = {value}");
            Console.WriteLine($"min {userChoice} is for planet:" + $"{minStat.Name} with value = {value}");
        }

        /***********this is one way of doing this with fewer params, there's another way***************************************
        private void ShowStatistics(string? userChoice, IEnumerable<Planet> items)
        {
            var propInfo = typeof(Planet).GetProperty(userChoice);
            if (propInfo == null)
            {
                Console.WriteLine($"Property '{userChoice}' not found on type {typeof(Planet).Name}.");
            }
            else
            {
                Console.WriteLine($"Property '{userChoice}' found on type {typeof(Planet).Name}.");
            }

            var maxStat = items.MaxBy(p => propInfo.GetValue(p));
            var minStat = items.MinBy(p => propInfo.GetValue(p));
            Console.WriteLine($"max {userChoice} is for planet:" + $"{maxStat.Name} with value = {propInfo.GetValue(maxStat)}");
            Console.WriteLine($"min {userChoice} is for planet:" + $"{minStat.Name} with value = {propInfo.GetValue(minStat)}");
        }

        *************************************************************************************************************************/

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
            public string Name { get; }
            public int Diameter { get; }
            public int? SurfaceWater { get; }
            public int? Population { get; }

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



