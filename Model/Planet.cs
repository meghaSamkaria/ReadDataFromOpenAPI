using StarWarsOpenAPI.DTOs;


namespace StarWarsOpenAPI
{
    public partial class StarWarsPlanetsStats
    {
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


    }


}



