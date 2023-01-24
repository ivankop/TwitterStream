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

Console.WriteLine("Hello, World!");



var serviceProvider = new ServiceCollection()
    // for demo purpuse we use in-memory repository here
    .AddSingleton<IRepository<Tweet>, InMemoryRepository>()
    // for demo purpuse we use in-memory repository here
    .AddSingleton<IKeyValueRepository<string, int>, InMemoryKeyValueRepository<string, int>>()
    // Add commands handlers
    .AddScoped<ICommandHandler<Tweet>, AddNewTweetCommandHandler>()
    .AddScoped<ICommandHandler<HashTagStatisticCommandHandlerEntity>, HashTagStatisticCommandHandler>()
    // Add query handlers
    .AddScoped<IQueryHandler<TotalTweetsQueryHandlerResult>, TotalTweetsQueryHandler>()
    .AddScoped<IQueryHandler<TopHashTagsQuery, TopHashTagsQueryHandlerResult>, TopHashTagsQueryHandler>()
    //Creat service
    .BuildServiceProvider();

var commandDispatcher = new CommandDispatcher(serviceProvider);
var queryDispatcher = new QueryDispatcher(serviceProvider);


CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
CancellationToken token = cancelTokenSource.Token;
TwitterStreamReader twitterStreamReader = new TwitterStreamReader();
twitterStreamReader.NewTweet += TwitterStreamReader_NewTweet;
// start reading form Twitter sample stream
twitterStreamReader.Start(token);


Console.ReadLine();


cancelTokenSource.Cancel();
Console.ReadLine();
var totalTweetResult = queryDispatcher.Send<TotalTweetsQueryHandlerResult>();
Console.WriteLine($"Total tweets: {totalTweetResult.Total}");


var toptenTags = queryDispatcher.Send<TopHashTagsQuery, TopHashTagsQueryHandlerResult>(new TopHashTagsQuery(10));
Console.WriteLine($"Top {toptenTags.Tags.Count} hashtags");
foreach (var tag in toptenTags.Tags)
{
    Console.WriteLine(tag);
}

void TwitterStreamReader_NewTweet(Tweet tweet)
{
    // Console.WriteLine(tweet.text);
    commandDispatcher.Send(tweet);

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
}