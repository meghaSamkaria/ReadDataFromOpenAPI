namespace StarWarsOpenAPI
{
    public static class StringExtensions
    {
        public static int? ToIntOrNull(this string? toBeConverted)   //making this an extension method as this has NO role in starwarstat class
        {
            return int.TryParse(toBeConverted, out int parsedType) ? parsedType : null;        
        }
    }


}



