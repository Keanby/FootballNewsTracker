using DBRepository;
using DBServices;
using Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Xml.Linq;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IModel channel;
        private readonly string consumer_exchange;
        private readonly INewsService newsService;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            // Init RabbitMQ
            string host_name = Environment.GetEnvironmentVariable("RabbitMQHost") ?? throw new ArgumentException("Missing env var: RabbitMQHost");
            consumer_exchange = Environment.GetEnvironmentVariable("ConsumerExchange") ?? throw new ArgumentException("Missing env var: ConsumerExchange");

            

            channel = new ConnectionFactory { HostName = host_name }.CreateConnection().CreateModel();
            channel.ExchangeDeclare(exchange: consumer_exchange, type: ExchangeType.Direct);

            var newsConsumerQueue = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: newsConsumerQueue, exchange: consumer_exchange,routingKey: String.Empty);

            //var newNewsConsumer = new EventingBasicConsumer(channel);
            //newNewsConsumer.Received += RecieveNews;
            //channel.BasicConsume(queue: newsConsumerQueue, autoAck: true, consumer: newNewsConsumer);


            // init postgres service
            string postgres_connectionstring = Environment.GetEnvironmentVariable("PostgresConnectionString") ?? throw new ArgumentException("Missing env var: PostgresConnectionString");
            newsService = new NewsService(new NewsRepository(postgres_connectionstring));
            _logger.LogInformation("Working...");


            var newNewsConsumer = new EventingBasicConsumer(channel);
            newNewsConsumer.Received += RecieveNews;
            channel.BasicConsume(queue: newsConsumerQueue, autoAck: true, consumer: newNewsConsumer);




        }
        public void RecieveNews(object? sender, BasicDeliverEventArgs e)
        {
            _logger.LogInformation("RecieveNews called");
            string dataStr = Encoding.UTF8.GetString(e.Body.ToArray());
            _logger.LogInformation("Data: ", dataStr);
            var news = JsonConvert.DeserializeObject<List<NewsDto>>(dataStr);
            foreach (var element in news)
            {
                _logger.LogInformation("Title: {title}, link: {link}, time: {time}",
                    element.Title,
                    element.Url,
                    element.Time);
                var task = newsService.AddNewsAsync(new Dtos.Database.NewsDto { Link = element.Url, Time = element.Time, Title = element.Title, AddDateTime = DateTime.Now });
                task.Wait(10000);
                if(task.IsCompletedSuccessfully)
                {

                    var db_element = task.Result;
                    if(db_element is not null)
                    {
                        _logger.LogInformation("Add to database with Id: {id} Title: {title}, link: {link}, time: {time}",
                                                db_element.Id,
                                                db_element.Title,
                                                db_element.Link,
                                                db_element.Time);
                    }
                }
                else
                {
                    _logger.LogError("Task exception: ",task.Exception.Message);
                }

            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}