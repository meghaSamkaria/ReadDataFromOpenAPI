using static StarWarsOpenAPI.StarWarsPlanetsStats;


namespace StarWarsOpenAPI
{
    public class PlanetsStatsUserInteractor : IPlanetsStatsUserInteractor
    {
        private readonly IUserInteractor? _userInteractor;
        public PlanetsStatsUserInteractor(IUserInteractor? userInteractor)
        {
            _userInteractor = userInteractor;
        }
        public string ChooseStatisticsToBeShown(
            IEnumerable<string> PropertiesThatCanBeShown)
        {
            _userInteractor?.showMessage("The statistics of which property would you like to see?\n");
            _userInteractor?.showMessage(String.Join("\n", PropertiesThatCanBeShown));
            return _userInteractor?.ReadFromUser() ?? String.Empty;
        }
        public void show(IEnumerable<Planet> planets)
        {
            foreach (var planet in planets)
            {
                _userInteractor?.showMessage(planet.ToString());
            }
        }
        public void showMessage(string message)
        {
            _userInteractor?.showMessage(message);
        }
    }


}



