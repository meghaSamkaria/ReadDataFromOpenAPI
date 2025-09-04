using static StarWarsOpenAPI.StarWarsPlanetsStats;


namespace StarWarsOpenAPI
{
    public class PlanetsStatisticsAnalyzer : IPlanetsStatisticsAnalyzer
    {
        private readonly IPlanetsStatsUserInteractor? _PlanetsStatUserInteractor;

        public PlanetsStatisticsAnalyzer(IPlanetsStatsUserInteractor planetsStatUserInteractor)
        {
            _PlanetsStatUserInteractor = planetsStatUserInteractor;
        }

        public void Analyze(IEnumerable<Planet> planets)
        {
            var mapFromPropString2Func = new Dictionary<string, Func<Planet, int?>>()
                {
                    { "Population", planet => planet.Population },
                    { "Diameter", planet => planet.Diameter },
                    { "SurfaceWater", planet => planet.SurfaceWater }
                };

           
            var userChoice = _PlanetsStatUserInteractor.
                                ChooseStatisticsToBeShown(mapFromPropString2Func.Keys);

            if (userChoice is null || !mapFromPropString2Func.ContainsKey(userChoice))
            {
                _PlanetsStatUserInteractor.showMessage("Invalid choice.");
            }
            else
            {
                ShowStatistics(userChoice, planets, mapFromPropString2Func[userChoice]);
            }
        }

        private void ShowStatistics(string userChoice, IEnumerable<Planet> planets, Func<Planet, int?> value)
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

        private  void ShowStatisticsBy(string v, Planet planet, string userChoice, Func<Planet, int?> value)
        {
            _PlanetsStatUserInteractor.showMessage($"{v} {userChoice} is for planet:" + $"{planet.Name} with value = {value(planet)}");
        }
    }


}



