namespace specials_api
{
    public class Settings
    {
        public string Database { get; set; }

        public string ConnectionString { get; set; }

        public string Container { get; set; }

        public bool IsContained { get; set; }

        public bool Development { get; set; }
    }
}