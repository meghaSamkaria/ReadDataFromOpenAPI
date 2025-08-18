using StarWarsOpenAPI.APIDataAccess;
using StarWarsOpenAPI.DTOs;
using System.Text.Json;
using static StarWarsOpenAPI.StarWarsPlanetsStats;
using System.Linq;


namespace StarWarsOpenAPI
{
    public static class StringExtensions
    {
        public static int? ToIntOrNull(this string? toBeConverted)   //making this an extension method as this has NO role in starwarstat class
        {
            return int.TryParse(toBeConverted, out int parsedType) ? parsedType : null;        
        }
    }
    public class StarWarsPlanetsStats(ApiDataReader reader, MockStarWarsApiDataReader secondaryReader)
    {
        private readonly IApiDataReader? _reader = reader;
        private readonly IApiDataReader? _secondaryReader = secondaryReader;
      
        public async Task Run()
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

            if (_secondaryReader != null)
            {
                json ??= await _secondaryReader.Read(baseaddr, restOfAPI);
            }


            if (json is not null)
            {
                var root = JsonSerializer.Deserialize<Root>(json);
                var planets = ToPlanets(root);

                Dictionary<string, Func<Planet, int?>> mapFromPropString2Func = new()
                {
                    { "Population", planet => planet.Population },
                    { "Diameter", planet => planet.Diameter },
                    { "SurfaceWater", planet => planet.SurfaceWater }
                };

                foreach (var planet in planets)
                {
                    Console.WriteLine(planet);
                }
                Console.WriteLine();
                Console.WriteLine("The statistics of which property would you like to see?\n");
                Console.WriteLine(String.Join("\n", mapFromPropString2Func.Keys));

                var userChoice = Console.ReadLine();


                if (userChoice is null || !mapFromPropString2Func.TryGetValue(userChoice, out var selector))
                {
                    Console.WriteLine("Invalid choice.");
                }
                else
                {
                    ShowStatistics(userChoice, planets, selector);
                }
            }
            else
            {
                Console.WriteLine("Json was still null");
            }
        }

        private static void ShowStatistics(string userChoice, IEnumerable<Planet> planets, Func<Planet, int?> value)
        {
            ShowStatisticsBy("Max",
                             planets.MaxBy(value),
                             userChoice,
                             value);

            ShowStatisticsBy("Min",
                             planets.MinBy(value),
                             userChoice,
                             value);

        }

        private static void ShowStatisticsBy(string v, Planet planet, string userChoice, Func<Planet, int?> value)
        {
            Console.WriteLine($"{v} {userChoice} is for planet:" + $"{planet.Name} with value = {value(planet)}");
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

        private static IEnumerable<Planet> ToPlanets(Root? root)
        {

            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            return root.results.Select(planetDTO => (Planet)planetDTO);

        }
        public readonly record struct Planet
        {
            public string Name { get; }
            public int Diameter { get; }
            public int? SurfaceWater { get; }
            public int? Population { get; }

            public Planet(string? name, int diameter, int? surfaceWater, int? population)
            {
                ArgumentNullException.ThrowIfNull(name);
                Name = name;
                Diameter = diameter;
                SurfaceWater = surfaceWater;
                Population = population;
            }

            public static explicit operator Planet(Results planetDto)
            {

                string? name = planetDto.name;
                int dia = Convert.ToInt32(planetDto.diameter);

                // int water = Convert.ToInt32(planetDto.surface_water); //use try parse as we dont want the water to contain anything if value is null

                int? water = planetDto.surface_water.ToIntOrNull();  // notice how this is being called now as its method of string now

                int? population = planetDto.population.ToIntOrNull();

                return new Planet(name, dia, water, population);
            }
        }

        //private static void PrettyPrint(Root? root)
        //{
        //    Console.WriteLine($"{"Name",-15}|{"Diameter",-15}|{"Surface Water",-15}|{"Population",-15}|");
        //    Console.WriteLine("------------------------------------------------------------------");
        //    foreach (var planet in root.results)
        //    {
        //        Console.Write($"{planet.name,-15}|{planet.diameter,-15}|");
        //        if (planet.surface_water != "unknown")
        //        {
        //            Console.Write($"{planet.surface_water,-15}|");
        //        }
        //        else
        //        {
        //            Console.Write($"{"",-15}|");
        //        }
        //        if (planet.population != "unknown")
        //        {
        //            Console.WriteLine($"{planet.population,-15}|");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"{"",-15}|");
        //        }
        //    }

        //}
    }
}



