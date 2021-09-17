using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;

namespace console_client
{
    class Data
    {
        public float AverageTweetsPerMinute {get; set;}
        public int TotalTweets {get; set;}
    }
    
    class Program
    {
        static CancellationTokenSource _tokenSource;
        static CancellationToken _cancelToken;


        static void Main(string[] args)
        {
            _tokenSource = new CancellationTokenSource();
            _cancelToken = _tokenSource.Token;

            Task.Run(() => GetTwitterAnalysis());
            
            Console.WriteLine("Press enter to exit...");

            Console.Read();
        }

        static async Task GetTwitterAnalysis()
        {
            HttpResponseMessage responseMessage;

            HttpClient http;

            Data data;

            try
            {

                http = new HttpClient();
                http.BaseAddress = new Uri("http://localhost:6000/");
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                while (!_cancelToken.IsCancellationRequested)
                {
                    responseMessage = await http.GetAsync("v1/twitteranalysis");
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        data = await responseMessage.Content.ReadAsAsync<Data>();

                        Console.WriteLine("*****************************************************************************");
                        Console.WriteLine($"Average Tweets Per Minute: {data.AverageTweetsPerMinute} Total Tweets: {data.TotalTweets}");
                        Console.WriteLine("*****************************************************************************");
                    }
                    else
                    {
                        Console.WriteLine("Error..no response from Web Api Service.");
                    }

                    await Task.Delay(1000, _cancelToken);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"Error: {err.Message}");
            }
        }
    }
}
