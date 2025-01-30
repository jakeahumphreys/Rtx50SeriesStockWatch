namespace Rtx50SeriesStockWatch.Common;

using System;
using System.Net.Http;
using System.Text;

public class CurlExporter
{
    public static string Export(HttpRequestMessage request)
    {
        var curlCommand = new StringBuilder("curl -X ");

        // Add the HTTP method
        curlCommand.Append(request.Method.ToString().ToUpper());

        // Add the URL
        curlCommand.Append($" '{request.RequestUri}'");

        // Add headers
        foreach (var header in request.Headers)
        {
            foreach (var value in header.Value)
            {
                curlCommand.Append($" -H '{header.Key}: {value}'");
            }
        }

        // Add query parameters
        if (request.RequestUri.Query.Length > 0)
        {
            curlCommand.Append($" '{request.RequestUri.Query}'");
        }

        // Add the request body
        if (request.Content != null)
        {
            var requestBody = request.Content.ReadAsStringAsync().Result;
            curlCommand.Append($" -d '{requestBody}'");
        }

        return curlCommand.ToString();
    }

    public static string ToCurl(HttpRequestMessage request)
    {
        var sb = new StringBuilder("curl");

        // Add method if not GET
        if (request.Method != HttpMethod.Get)
        {
            sb.Append($" -X {request.Method}");
        }

        // Add URL
        sb.Append($" \"{request.RequestUri}\"");

        // Add headers
        foreach (var header in request.Headers)
        {
            var headerValue = string.Join(",", header.Value); // Prevents multiple -H for same header
            sb.Append($" -H \"{header.Key}: {headerValue}\"");
        }

        if (request.Content != null)
        {
            foreach (var header in request.Content.Headers)
            {
                var headerValue = string.Join(",", header.Value);
                sb.Append($" -H \"{header.Key}: {headerValue}\"");
            }

            var body = request.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(body))
            {
                sb.Append($" --data-raw '{body}'");
            }
        }
        
        return sb.ToString();

    }

}
