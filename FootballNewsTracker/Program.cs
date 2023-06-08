using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var options = new ChromeOptions();
options.AddArgument("--ignore-certificate-errors");
using var driver = new ChromeDriver(@"C:\ChromeDriver\chromedriver_win32\chromedriver.exe", options);

driver.Navigate().GoToUrl("https://www.ua-football.com/ua");
// отримання заголовків новин футболу

var newsHeadlines = new List<Help.NewsElement>();
//var headlinesElements = driver.FindElements(By.CssSelector("div.title"));
//var headlinesElements = driver.FindElements(By.CssSelector(".news-list-item .title"));

//var headlinesElements = driver.FindElements(By.CssSelector(".news-list-item .title"));
//var headlinesElements = driver.FindElements(By.ClassName("fbi"));
var headlinesElements = driver.FindElements(By.CssSelector(".topnews-list.b-back-content li"));


foreach (var element in headlinesElements)
{
    try
    {
        var time_element = element.FindElement(By.CssSelector(".time.fz-12"));
        var title_element = element.FindElement(By.ClassName("name"));
        var link_element = element.FindElement(By.CssSelector("a"));
        var link = link_element.GetAttribute("href");

        //var title_element = element.FindElement(By.CssSelector("div.title"));
        //var link_element = element.FindElement(By.CssSelector("a"));
        //var time_element = element.FindElement(By.ClassName("time"));

        //var time = time_element.Text;
        //var link = link_element.GetAttribute("href");

        newsHeadlines.Add(new Help.NewsElement { Title = title_element.Text, Url = link, Time = time_element.Text });
    }
    catch (Exception)
    {


    }
    //newsHeadlines.Add(element.Text);
}
// виведення заголовків новин в консоль
Console.WriteLine("Football news headlines:");
foreach (var headline in newsHeadlines)
{
    Console.WriteLine(headline.Title);
}


namespace Help
{
    class NewsElement
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Time { get; set; }
    }
}


//using DBRepository;
//using DBService;
//using DBServices;
//using Dtos.Database;


////IUsersService usersService = new UsersService(new  UserRepository("Host=localhost;Port=5432;Database=FootBallNews;Username=postgres;Password=postgres"));
//INewsService newsService = new NewsService(new NewsRepository("Host=localhost;Port=5432;Database=FootBallNews;Username=postgres;Password=postgres"));

//newsService.AddNewsAsync(new NewsDto { Link = "link1", Title = "title", Time = "00:00", AddDateTime = DateTime.Now }).Wait();


////var add_user = await usersService.AddUserAsync(new UserDto { Username = "test3", Password = "test3" });
////var all_users =await usersService.GetAllUsersAsync();

//Console.WriteLine("Finish");