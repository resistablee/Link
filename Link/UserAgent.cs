using Link.Models;
using MyCSharp.HttpUserAgentParser;

namespace Link
{
    public class UserAgent
    {
        public static UserAgentModel Parse(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                throw new ArgumentNullException("userAgent");

            HttpUserAgentInformation parsedUserAgent = HttpUserAgentParser.Parse(userAgent);
            return new UserAgentModel
            {
                UserAgent = userAgent,
                Browser = new Browser
                {
                    Name = parsedUserAgent.Name,
                    Version = parsedUserAgent.Version
                },
                Platform = new Platform
                {
                    Name = parsedUserAgent.Platform.Value.Name,
                    Type = parsedUserAgent.Platform.Value.PlatformType.ToString(),
                },
            };
        }
    }
}