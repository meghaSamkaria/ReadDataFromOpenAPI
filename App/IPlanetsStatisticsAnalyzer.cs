using static StarWarsOpenAPI.StarWarsPlanetsStats;


namespace StarWarsOpenAPI
{
    public interface IPlanetsStatisticsAnalyzer
    {
        void Analyze(IEnumerable<Planet> planets);
    }


}



