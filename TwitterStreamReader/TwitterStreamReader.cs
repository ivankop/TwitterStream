using Newtonsoft.Json;
using System.Configuration;
using TwitterStream.Domain;

namespace TwitterStream
{
    public class TwitterStreamReader
    {
        private static string API_KEY;
        static TwitterStreamReader()
        {
            API_KEY = ConfigurationManager.AppSettings.Get("TWITTER_API_KEY");
        }
        public delegate void NewTweetDelegate(Tweet tweet);


        public event NewTweetDelegate NewTweet;

        public void Start(CancellationToken cancelToken)
        {

            Task task = new Task(() =>
            {
                StartReading(cancelToken);
            }, cancelToken);
            task.Start();
        }
        private async void StartReading(CancellationToken cancelToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                var requestUri = "https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("Authorization", $"Bearer {API_KEY}");

                var response = httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, cancelToken).Result;
                var stream = await response.Content.ReadAsStreamAsync(cancelToken);
                int count = 0;
                using (var reader = new StreamReader(stream))
                {

                    while (!reader.EndOfStream && !cancelToken.IsCancellationRequested)
                    {
                        var currentLine = reader.ReadLine();
                        if (!string.IsNullOrEmpty(currentLine))
                        {
                            try
                            {
                                var tweetData = JsonConvert.DeserializeObject<TweetData>(currentLine);
                                if (tweetData!= null)
                                {
#if DEBUG
                                    Console.Write("\rReceived {0} tweets", ++count);
#endif
                                    NewTweet(tweetData.data);
                                }
                                
                            }
                            catch (Exception)
                            {
                                // TO DO: handle parsing error
                                throw;
                            }
                        }
                    }
                }
            }
        }
    }
}
