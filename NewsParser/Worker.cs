using AngleSharp.Dom;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;

namespace NewsParser
{
    public class NewsParserWorker : BackgroundService
    {
        private readonly ILogger<NewsParserWorker> _logger;
        private ChromeOptions _chromeOptions;

        private readonly IModel channel;
        private readonly string consumer_exchange;

        public NewsParserWorker(ILogger<NewsParserWorker> logger)
        {

            _chromeOptions = new ChromeOptions();
            _chromeOptions.AddArgument("--headless");
            _chromeOptions.AddArgument("--no-sandbox");
            _chromeOptions.AddArgument("--disable-dev-shm-usage");

            // Init RabbitMQ
            string host_name = Environment.GetEnvironmentVariable("RabbitMQHost") ?? throw new ArgumentException("Missing env var: RabbitMQHost");
            consumer_exchange = Environment.GetEnvironmentVariable("ConsumerExchange") ?? throw new ArgumentException("Missing env var: ConsumerExchange");

            channel =  new ConnectionFactory { HostName = host_name }.CreateConnection().CreateModel();
            channel.ExchangeDeclare(exchange: consumer_exchange, type: ExchangeType.Direct);


            _logger = logger;
        }

        private void ParseNews()
        {
            var newsHeadlines = new List<Dtos.NewsDto>();
            using (var driver = new ChromeDriver(@"/chromedriver", _chromeOptions))
            {
                driver.Navigate().GoToUrl("https://www.ua-football.com/ua");
                //var headlinesElements = driver.FindElements(By.ClassName("fbi"));
                //var headlinesElements = driver.FindElements(By.CssSelector(".topnews-list.b-back-content"));
                var headlinesElements = driver.FindElements(By.CssSelector(".topnews-list.b-back-content li"));


                foreach (var element in headlinesElements)
                {
                    try
                    {
                        var time = element.FindElement(By.CssSelector(".time.fz-12")).Text;
                        var title = element.FindElement(By.ClassName("name")).Text;
                        var link_element = element.FindElement(By.CssSelector("a"));
                        var link = link_element.GetAttribute("href");
                        var newsElement = new Dtos.NewsDto { Title = title, Url = link, Time = time };
                        newsHeadlines.Add(newsElement);
                        _logger.LogInformation(
                            "Title: {title}, link: {link}, time: {time}",
                            newsElement.Title,
                            newsElement.Url,
                            newsElement.Time);
                    }
                    catch(Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
            }
            channel.BasicPublish(
                exchange: consumer_exchange,
                routingKey: String.Empty,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newsHeadlines))
                );

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                this.ParseNews();
                await Task.Delay(300000, stoppingToken);
            }
        }
    }
}