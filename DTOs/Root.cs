namespace StarWarsOpenAPI.DTOs;

public class Root
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<Results> results { get; set; }
    }
