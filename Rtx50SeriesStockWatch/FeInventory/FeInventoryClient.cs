using System.Net;
using Newtonsoft.Json;
using Rtx50SeriesStockWatch.FeInventory.Types;

namespace Rtx50SeriesStockWatch.FeInventory;

public sealed class FeInventoryClient
{
    private const int REQUEST_TIMEOUT_SECONDS = 20;
    
    public async Task<FeInventoryResponse> GetInventoryAsync(string skus, string locale)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.store.nvidia.com/partner/v1/feinventory?skus={skus}&locale={locale}");

        //Need these specific headers it seems to get an "authenticated" response
        request.Headers.Clear();
        request.Headers.Add("accept", "application/json, text/plain, */*");
        request.Headers.Add("accept-language", "en-GB,en;q=0.6");
        request.Headers.Add("origin", "https://marketplace.nvidia.com");
        request.Headers.Add("referer", "https://marketplace.nvidia.com");
        request.Headers.Add("sec-ch-ua", "\"Not A(Brand\";v=\"8\", \"Chromium\";v=\"132\", \"Brave\";v=\"132\"");
        request.Headers.Add("sec-ch-ua-mobile", "?0");
        request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
        request.Headers.Add("sec-fetch-dest", "empty");
        request.Headers.Add("sec-fetch-mode", "cors");
        request.Headers.Add("sec-fetch-site", "cross-site");
        
        //Needs Http2, 1.0/1.1 times out
        request.Version = HttpVersion.Version20;
        request.VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;

        //spoof user agent
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
        
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };
        
        using (var client = new HttpClient(handler))
        {
            //Needs Http2, 1.0/1.1 times out
            client.DefaultRequestVersion = HttpVersion.Version20;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher;
            
            client.Timeout = TimeSpan.FromSeconds(REQUEST_TIMEOUT_SECONDS);
            var response = await client.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
            }
            
            var content = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<FeInventoryResponse>(content)!;
        }
    }
}