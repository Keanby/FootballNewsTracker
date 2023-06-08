using DBRepository;
using DBService;
using DBServices;
using GraphQL.Classes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string postgres_connectionstring = Environment.GetEnvironmentVariable("PostgresConnectionString") ?? throw new ArgumentException("Missing env var: PostgresConnectionString");
var redis_connectionstring = Environment.GetEnvironmentVariable("RedisConnectionString") ?? throw new ArgumentException("Missing env var: RedisConnectionString");


var jwtIssuer = Environment.GetEnvironmentVariable("JWT_AUTH_ISSUER") ?? throw new ArgumentException("Missing env var: AUTH_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUTH_AUDIENCE") ?? throw new ArgumentException("Missing env var: AUTH_AUDIENCE");
var jwtAuthKey =  Environment.GetEnvironmentVariable("JWT_AUTH_KEY") ?? throw new ArgumentException("Missing env var: JWT_AUTH_KEY");



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


// AddAuthentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthKey)),

        ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();


builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .RegisterService<INewsService>()
    .RegisterService<IUsersService>()
    .RegisterService<IWatchLaterService>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscriptions>()
    .AddFiltering()
    .AddRedisSubscriptions((sp) =>
    ConnectionMultiplexer.Connect(redis_connectionstring));


var app = builder.Build();





app.MapGet("/", () => "Hello World!");

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();
app.MapGraphQL();

app.Run();
