namespace TwitterStream.Domain
{
    public class Annotation
    {
        public int start { get; set; }
        public int end { get; set; }
        public double probability { get; set; }
        public string type { get; set; }
        public string normalized_text { get; set; }
    }

    public class Cashtag
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class TweetData
    {
        public Tweet data { get; set; }
    }

    public class Tweet
    {
        public string author_id { get; set; }
        public DateTime created_at { get; set; }
        public string id { get; set; }
        public List<string> edit_history_tweet_ids { get; set; }
        public string text { get; set; }
        public Entities entities { get; set; }
    }

    public class Entities
    {
        public List<Annotation> annotations { get; set; }
        public List<Cashtag> cashtags { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public List<Mention> mentions { get; set; }
        public List<Url> urls { get; set; }
    }

    public class Hashtag
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class Includes
    {
        public List<User> users { get; set; }
    }

    public class Mention
    {
        public int start { get; set; }
        public int end { get; set; }
        public string tag { get; set; }
    }

    public class Root
    {
        public List<Tweet> data { get; set; }
        public Includes includes { get; set; }
    }

    public class Url
    {
        public int start { get; set; }
        public int end { get; set; }
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string unwound_url { get; set; }
    }

    public class User
    {
        public DateTime created_at { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }
}