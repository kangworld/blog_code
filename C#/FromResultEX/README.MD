# ✍️ Task.FromResult

await 키워드를 만나면 세 가지 경우의 수가 발생한다. 

첫 번째, awaitable이 실행 과정에서 예외를 발생하면 exception을 던진다.

> awaitable  
> 간단하게 Task 또는 Task<T>를 반환하는 함수(+메서드)라고 생각하면 된다.   
>   
> +awaitable은 void도 반환하지만 Task를 반환하는 awaitable과는 결이 다르다고 한다.  
>   
> 자세한 내용은 [링크](https://docs.hhvm.com/hack/asynchronous-operations/awaitables)를 참조하길 바랍니다.

두 번째, awaitable이 이미 완료된 Task라면 async 메서드를, 마치 일반 메서드처럼, 동기 방식으로 계속 실행한다.  

세 번째, awaitable이 끝나지 않았다면 작업이 끝난 후 await 이후의 나머지 코드를 실행하도록 대기 작업으로 등록하고  async 메서드의 호출자에게 Task를 반환한다.  

본 포스팅은 Task.FromResult를 다루며 이는 두 번째 상황과 관련이 있다.

Task 클래스 내부의 CompletedTask 프로퍼티는 작업이 완료된 더미 Task를 반환한다.

나아가 완료된 더미 Task에 어떠한 값을 실어서 보내고 싶다면 특정 메서드를 사용하면 되는데 이게 바로 본 포스티에서 다룰 Task.FromResult 메서드이다.

상식선에서 생각해 봤을 때 완료된 Task는 비동기식으로 실행할 필요가 없다. 왜냐? 이미 끝났으니 말이다.

그렇다면 Task 클래스는 어떤 이유로 완료된 더미 Task를 제공하는 걸까?

여기에 대한 답은 바로 캐싱과 관련이 있다. 

# 🤔 Task.FromResult와 캐싱

예를 들어 비동기적으로 서버로부터 유저의 닉네임을 받아오는 GET 메서드가 있다.

개발자 K 씨는 A라는 유저의 닉네임이 필요해 GET 메서드를 호출했다. 

K 씨는 이것저것 코드를 작성하다가 유저 A의 닉네임이 필요해졌다. 

또다시 유저 A의 닉네임이 필요한 상황... 개발자 K 씨는 이 상황을 어떻게 풀어갈까?

첫 번째, 다시 GET 메서드를 호출해서 닉네임을 얻어오면 매우 쉽다. 그런데 서버로부터 어떤 값을 얻어오는 작업엔 적지 않은 시간이 걸리므로 이를 반복적으로 수행하는 건 문제가 있어보인다.

두 번째, 일종의 캐싱으로 딕셔너리를 선언해서 유저의 닉네임이 딕셔너리에 없다면 1회에 한정해 값을 서버로부터 받아와 딕셔너리에 저장한다. 이후에 유저의 닉네임이 필요하면 딕셔너리에서 값을 빼오면 된다.

다르게 말하면, GET 메서드 내부에서 키에 해당하는 Value가 있다면 Task.FromResult 메서드를 이용해 값을 즉시 반환하면 되고 없다면 비동기적으로 서버로부터 값을 얻어오면 된다. (데이터의 동기화에 관한 문제는 남아있지만 괜찮은 해결법 같다.)

# 🍊 예제 코드

MS Docs에 좋은 예제가 있어서 가져왔다.

코드의 흐름은 위에서 설명한 것과 매우 유사하다.

[예제 코드 링크](https://docs.microsoft.com/ko-kr/dotnet/standard/parallel-programming/how-to-create-pre-computed-tasks)

```cs
using System;

namespace AsyncEX
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
```

팁은 아래와 같이 Task.Run과 람다식을 이용하면 동기 메서드 내에서 비동기 메서드를 호출할 수 있다는 것이다!

```cs
return Task.Run(async () =>
{
      content = await new WebClient().DownloadStringTaskAsync(address);
      cachedDownloads.TryAdd(address, content);
      return content;
});
```
