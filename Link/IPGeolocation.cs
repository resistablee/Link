using Figensoft.NET.Framework.Caching.Interfaces;
using Figensoft.NET.Framework.Configuration.Interface;
using Figensoft.NET.Framework.Enums;
using Figensoft.NET.Framework.Models;
using Link.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Link
{
    public class IPGeolocation
    {
        private readonly ICaching _Caching;
        private readonly IConfig _Config;

        public IPGeolocation(ICaching caching, IConfig config)
        {
            _Caching = caching;
            _Config = config;
        }

        public AppResponse<IPGeolocationModel> Check(string ip)
        {
            // IP adresini doğrula
            if (!ValidateIPAddress(ip))
                return AppResponse<IPGeolocationModel>.Failed("Invalid IP address");

            // IP Geolocation bilgilerini cache'den kontrol et
            AppResponse<IPGeolocationModel> cacheData = CheckCache(ip);
            if (StatusCode.SUCCEED.Equals(cacheData))
                return cacheData;

            // IP Geolocation bilgilerini API'den çek
            AppResponse<IPGeolocationModel> ipLocate = RequestIPGeolocation(ip);
            if (!StatusCode.SUCCEED.Equals(ipLocate))
                return ipLocate;

            // IP Geolocation bilgilerini cache'e kaydet
            SaveToCache(ip, ipLocate.Result);

            return ipLocate;
        }

        // IP adresini doğrula
        private bool ValidateIPAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            if (!IPAddress.TryParse(ip, out _))
                return false;

            return true;
        }

        // Cache'den IP adresi bilgilerini kontrol et
        private AppResponse<IPGeolocationModel> CheckCache(string ip)
        {
            string[] ipSplit = ip.Split('.');
            string memoryCacheKey = $"{ipSplit[0]}.{ipSplit[0]}.{ipSplit[0]}";

            var cacheData = _Caching.Get<IPGeolocationModel>(memoryCacheKey);
            if (cacheData == null)
                return AppResponse<IPGeolocationModel>.NotFound();

            return AppResponse<IPGeolocationModel>.Success(cacheData);
        }

        // IP Geolocation bilgilerini API'den çek
        private AppResponse<IPGeolocationModel> RequestIPGeolocation(string ip)
        {
            string apiUrl = _Config.GetString("APIIP:ApiUrl");
            string accessKey = _Config.GetString("APIIP:AccessKey");

            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(accessKey))
                return AppResponse<IPGeolocationModel>.Failed("APIIP configuration is missing");

            apiUrl = apiUrl.Replace("{IP}", ip);
            apiUrl = apiUrl.Replace("{ACCESS_KEY}", accessKey);

            HttpResponseMessage responseMessage;
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(apiUrl);

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        responseMessage = client.Send(request);
                    }
                    catch (Exception ex)
                    {
                        return AppResponse<IPGeolocationModel>.ServerError($"APIIP IP geolocation api request exception. Ex message: {ex.Message}");
                    }
                }
            }
            string responseString = responseMessage.Content.ReadAsStringAsync().Result;

            IPGeolocationModel ipGeolocation;
            try
            {
                ipGeolocation = JsonSerializer.Deserialize<IPGeolocationModel>(responseString);
            }
            catch (Exception ex)
            {
                return AppResponse<IPGeolocationModel>.ServerError($"APIIP IP geolocation api response deserialization exception. Ex message: {ex.Message}");
            }

            return AppResponse<IPGeolocationModel>.Success(ipGeolocation);
        }

        // IP Geolocation bilgilerini cache'e kaydet
        private void SaveToCache(string ip, IPGeolocationModel ipLocate)
        {
            string[] ipSplit = ip.Split('.');
            string memoryCacheKey = $"{ipSplit[0]}.{ipSplit[1]}.{ipSplit[2]}";

            _Caching.Set(memoryCacheKey, ipLocate, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30),
            });
        }
    }
}