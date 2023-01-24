# Twitter Sample Stream Reader

The sample application is in the Applications folder. Please make sure to update *TWITTER_API_KEY* in *App.Config* *in TwitterStreamReader* project.

The reader will run, collect data and save data to an in-memory repository. Press Enter to stop the reader and it will print out these two values:

-  Total number of tweets received
-  Top 10 Hashtags

In this sample, I'm using CQRS pattern with in-memory repositories. In real-world, we can replace those with SQL Server, Redis or any database.

Van Pham