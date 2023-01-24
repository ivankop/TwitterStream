// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using TwitterStream;
using TwitterStream.CommandHandlers;
using TwitterStream.Contract.RepositoryContract;
using TwitterStream.Domain;
using TwitterStream.Interfaces.Command;
using TwitterStream.Interfaces.Query;
using TwitterStream.QueryHandlers;
using TwitterStream.Repository;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Twitter Sample Stream Reader");

var serviceProvider = new ServiceCollection()
    // for demo purpose we use in-memory repository here
    .AddSingleton<IRepository<Tweet>, InMemoryRepository>()
    // using key-value repository to store statistic data
    .AddSingleton<IKeyValueRepository<string, int>, InMemoryKeyValueRepository<string, int>>()
    // Add commands handlers
    .AddScoped<ICommandHandler<Tweet>, AddNewTweetCommandHandler>()
    .AddScoped<ICommandHandler<HashTagStatisticCommandHandlerEntity>, HashTagStatisticCommandHandler>()
    // Add query handlers
    .AddScoped<IQueryHandler<TotalTweetsQueryHandlerResult>, TotalTweetsQueryHandler>()
    .AddScoped<IQueryHandler<TopHashTagsQuery, TopHashTagsQueryHandlerResult>, TopHashTagsQueryHandler>()
    //Create service
    .BuildServiceProvider();

var commandDispatcher = new CommandDispatcher(serviceProvider);
var queryDispatcher = new QueryDispatcher(serviceProvider);

CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
TwitterStreamReader twitterStreamReader = new TwitterStreamReader();
// listening to new tweet event
twitterStreamReader.NewTweet += TwitterStreamReader_NewTweet;
// start reading form Twitter sample stream
twitterStreamReader.Start(token);

Console.WriteLine($"Press Enter to stop reader process");
Console.ReadLine();

// cancel reader stream
cancelTokenSource.Cancel();


// print out results
var totalTweetResult = queryDispatcher.Send<TotalTweetsQueryHandlerResult>();
Console.WriteLine($"Total tweets: {totalTweetResult.Total}");


var toptenTags = queryDispatcher.Send<TopHashTagsQuery, TopHashTagsQueryHandlerResult>(new TopHashTagsQuery(10));
Console.WriteLine($"Top {toptenTags.Tags.Count} hashtags");
foreach (var tag in toptenTags.Tags)
{
    Console.WriteLine(tag);
}

Console.ReadLine();
// End


/// <summary>
/// New tweet event handler
/// </summary>
void TwitterStreamReader_NewTweet(Tweet tweet)
{
    Task savingTask = new Task(() =>
    {
        // save to repository
        commandDispatcher.Send(tweet);
    }, token);
    savingTask.Start();

    Task updateStatisticTask = new Task(() =>
    {
        // update statistic
        if (tweet?.entities?.hashtags != null)
        {
            foreach (var hashtag in tweet.entities.hashtags)
            {
                if (!string.IsNullOrEmpty(hashtag.tag))
                {
                    HashTagStatisticCommandHandlerEntity entity = new HashTagStatisticCommandHandlerEntity(hashtag.tag);
                    commandDispatcher.Send(entity);
                }
            }
        }
    }, token);
    updateStatisticTask.Start();
}