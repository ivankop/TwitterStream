using Newtonsoft.Json;
using TwitterStream.Domain;

namespace TwitterStream
{
    public class TwitterStreamReader
    {
        public delegate void NewTweetDelegate(Tweet tweet);

        public event NewTweetDelegate NewTweet ;

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

                /*
                var formUrlEncodedContent = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>() {
                        new KeyValuePair<string, string>("userId", "1000") });

                formUrlEncodedContent.Headers.ContentType =
                    new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                */
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                // request.Content = formUrlEncodedContent;
                request.Headers.Add("Authorization", "Bearer AAAAAAAAAAAAAAAAAAAAAAshlQEAAAAAOo%2F0yGgAb8DNusdELSf6N%2F4QDDI%3D8kQweHRhmEeVSPK63rlXohbKZtctE2GBOlGoUdddgu7uGFvjdu");
                // request.Headers.Add("Cookie", "guest_id=v1%3A167416813583863635; guest_id_ads=v1%3A167416813583863635; guest_id_marketing=v1%3A167416813583863635; personalization_id=\"v1_YbvEFpqVJHB9WzZcqTphzQ==\"");


                var response = httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead, cancelToken).Result;
                var stream = response.Content.ReadAsStreamAsync(cancelToken).Result;
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
                                    Console.Write("\r{0}", ++count);
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
            Console.WriteLine("Stopping");
        }
    }
}
