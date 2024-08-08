using System.Text.Json.Serialization;

namespace Link.Models
{
    public class IPGeolocationModel
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("continentCode")]
        public string ContinentCode { get; set; }

        [JsonPropertyName("continentName")]
        public string ContinentName { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }

        [JsonPropertyName("countryName")]
        public string CountryName { get; set; }

        [JsonPropertyName("countryNameNative")]
        public string CountryNameNative { get; set; }

        [JsonPropertyName("officialCountryName")]
        public string OfficialCountryName { get; set; }

        [JsonPropertyName("regionCode")]
        public string RegionCode { get; set; }

        [JsonPropertyName("regionName")]
        public string RegionName { get; set; }

        [JsonPropertyName("cityGeoNameId")]
        public int CityGeoNameId { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("capital")]
        public string Capital { get; set; }

        [JsonPropertyName("phoneCode")]
        public string PhoneCode { get; set; }

        [JsonPropertyName("countryFlagEmoj")]
        public string CountryFlagEmoj { get; set; }

        [JsonPropertyName("countryFlagEmojUnicode")]
        public string CountryFlagEmojUnicode { get; set; }

        [JsonPropertyName("isEu")]
        public bool IsEu { get; set; }

        [JsonPropertyName("borders")]
        public List<string> Borders { get; set; }

        [JsonPropertyName("topLevelDomains")]
        public List<string> TopLevelDomains { get; set; }
    }
}