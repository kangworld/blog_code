using System;

namespace FromResultEX
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    class CachedDownloads
    {
        // 다운로드된 결과물 저장하는 캐시
        static ConcurrentDictionary<string, string> cachedDownloads =
           new ConcurrentDictionary<string, string>();

        // 요청을 비동기적으로 수행하는 메서드
        public static Task<string> DownloadStringAsync(string address)
        {
            // 캐시에 존재하는지 확인
            string content;
            if (cachedDownloads.TryGetValue(address, out content))
            {
                return Task.FromResult<string>(content);
            }

            // 캐시에 없다면 다운하고 캐시에 저장
            return Task.Run(async () =>
            {
                content = await new WebClient().DownloadStringTaskAsync(address);
                cachedDownloads.TryAdd(address, content);
                return content;
            });
        }

        static void Main(string[] args)
        {
            // 다운받을 URL 주소
            string[] urls = new string[]
            {
         "http://www.naver.com",
         "http://www.google.com"
            };

            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            var downloads = from url in urls
                            select DownloadStringAsync(url);

            // 캐시를 활용하지 못하는 케이스
            Task.WhenAll(downloads).ContinueWith(results =>
            {
                stopwatch.Stop();
                Console.WriteLine("Retrieved {0} characters. Elapsed time was {1} ms.",
                   results.Result.Sum(result => result.Length),
                   stopwatch.ElapsedMilliseconds);
            })
            .Wait();

            // 캐시를 활용하는 케이스
            stopwatch.Restart();
            downloads = from url in urls
                        select DownloadStringAsync(url);
            Task.WhenAll(downloads).ContinueWith(results =>
            {
                stopwatch.Stop();

                Console.WriteLine("Retrieved {0} characters. Elapsed time was {1} ms.",
                   results.Result.Sum(result => result.Length),
                   stopwatch.ElapsedMilliseconds);
            })
            .Wait();
        }
    }
}
