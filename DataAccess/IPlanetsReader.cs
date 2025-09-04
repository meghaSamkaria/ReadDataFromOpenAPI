namespace StarWarsOpenAPI
{
    public interface IPlanetsReader
    {
        Task<IEnumerable<StarWarsPlanetsStats.Planet>> Read();
    }


}



