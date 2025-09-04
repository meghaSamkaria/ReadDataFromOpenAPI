using static StarWarsOpenAPI.StarWarsPlanetsStats;


namespace StarWarsOpenAPI
{
    public interface IPlanetsStatsUserInteractor
    {
        void show(IEnumerable<Planet> planets);
        string ChooseStatisticsToBeShown( 
            IEnumerable<String> PropertiesThatCanBeShown);
        
        void showMessage(string message);
    }


}



