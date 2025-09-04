using System.Linq;
using System.Reflection.PortableExecutable;


namespace StarWarsOpenAPI
{
    public partial class StarWarsPlanetsStats
    {

        private readonly PlanetFromApiReader _planetFromApiReader;
        private PlanetsStatisticsAnalyzer planetsStatisticsAnalyzer;
        private PlanetsStatsUserInteractor planetsStatsUserInteractor;

        public StarWarsPlanetsStats(PlanetFromApiReader planetFromApiReader, PlanetsStatisticsAnalyzer planetsStatisticsAnalyzer, PlanetsStatsUserInteractor planetsStatsUserInteractor)
        {
            _planetFromApiReader = planetFromApiReader;
            this.planetsStatisticsAnalyzer = planetsStatisticsAnalyzer;
            this.planetsStatsUserInteractor = planetsStatsUserInteractor;
        }


        public async Task Run()
        {
            var planets = await _planetFromApiReader.Read();
            planetsStatsUserInteractor.show(planets);
            planetsStatisticsAnalyzer.Analyze(planets);

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



