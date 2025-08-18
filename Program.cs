using StarWarsOpenAPI.APIDataAccess;
using System;
using System.Data;
using System.Net.Http;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarWarsOpenAPI
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await new StarWarsPlanetsStats(new ApiDataReader(),new MockStarWarsApiDataReader()).Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

       
    }
}



