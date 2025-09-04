namespace StarWarsOpenAPI
{
    public class ConsoleUserInteractor : IUserInteractor
    {
        public string? ReadFromUser()
        {
            return Console.ReadLine();
        }
        public void showMessage(string message)
        {
            Console.WriteLine(message);
        }
    }


}



