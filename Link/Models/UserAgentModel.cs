namespace Link.Models
{
    public class UserAgentModel
    {
        public string UserAgent { get; set; }
        public Browser Browser { get; set; }
        public Platform Platform { get; set; }
    }

    public class Browser
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class Platform
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
    }
}