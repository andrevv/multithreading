using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Multithreading.Async
{
    internal static class Program
    {
        static async Task Main()
        {
            var urls = new[]
            {
                "https://www.google.com",
                "https://www.yandex.com",
                "https://www.bing.com",
                "https://www.duckduckgo.com"
            };

            var httpClient = new HttpClient();
            
            // ...

            var httpResult = await httpClient.GetAsync("...");
            
            // ...

            Console.WriteLine(httpResult);
            
            CheckUrls(urls);
//            await CheckUrlAsync(urls[0]);
//            await CheckUrlsAsync(urls);
        }

        private static void CheckUrls(IEnumerable<string> urls)
        {
            var sw = Stopwatch.StartNew();

            foreach (var url in urls)
            {
                var uri = new Uri(url);
                var httpRequest = WebRequest.CreateHttp(uri);
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                Console.WriteLine($"{uri.Host} - {httpResponse.StatusCode}.");
            }

            sw.Stop();

            Console.WriteLine($"Done in {sw.Elapsed}.");
        }

        private static async Task CheckUrlAsync(string url)
        {
            var httpClient = new HttpClient();

            var httpResponse = await httpClient.GetAsync(new Uri(url));

            Console.WriteLine($"{httpResponse.RequestMessage.RequestUri.Host} - {httpResponse.StatusCode}.");
        }

        private static async Task CheckUrlsAsync(IEnumerable<string> urls)
        {
            var sw = Stopwatch.StartNew();

            var httpClient = new HttpClient();
            
            var tasks = new List<Task<HttpResponseMessage>>();
            foreach (var url in urls)
            {
                var task = httpClient.GetAsync(new Uri(url));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var result = await task;
                Console.WriteLine($"{result.RequestMessage.RequestUri.Host} - {result.StatusCode}");
            }

            sw.Stop();

            Console.WriteLine($"Done in {sw.Elapsed}.");
        }
    }
}