using DBRepository;
using DBService;
using DBServices;
using GraphQL.Classes;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

string postgres_connectionstring = Environment.GetEnvironmentVariable("PostgresConnectionString") ?? throw new ArgumentException("Missing env var: PostgresConnectionString");
var redis_connectionstring = Environment.GetEnvironmentVariable("RedisConnectionString") ?? throw new ArgumentException("Missing env var: RedisConnectionString");

builder.Services
    .AddTransient<INewsRepository>(news => new NewsRepository(postgres_connectionstring))
    .AddTransient<IUserRepository>(user => new UserRepository(postgres_connectionstring))
    .AddTransient<IWatchLaterRepository>(watch => new WatchLaterRepository(postgres_connectionstring));

builder.Services
    .AddTransient<INewsService, NewsService>()
    .AddTransient<IUsersService, UsersService>()
    .AddTransient<IWatchLaterService, WatchLaterService>();

// Enable cors
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options =>
    {
        options.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services
    .AddGraphQLServer()
    .RegisterService<INewsService>()
    .RegisterService<IUsersService>()
    .RegisterService<IWatchLaterService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscriptions>()
    .AddRedisSubscriptions((sp) =>
    ConnectionMultiplexer.Connect(redis_connectionstring));


var app = builder.Build();





app.MapGet("/", () => "Hello World!");

app.MapGraphQL();

app.Run();
