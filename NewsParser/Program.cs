using NewsParser;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<NewsParserWorker>();
    })
    .Build();

await host.RunAsync();
