# StreetPerfectClient for .Net
C# XPC Client for StreetPerfect native access

The easiest way to use the C# client is to add it as a singleton service in your ConfigureServices() aspnet function.

```
var sp_connectionString = Configuration.GetConnectionString("StreetPerfectServer");
var sp_debug = Configuration.GetSection("AppSettings:Debug").Value == "True";
services.AddSingleton(typeof(IStreetPerfectClient), new StreetPerfectClient(sp_connectionString, sp_debug));
```

You can then inject the service into any controller.

```
private readonly ILogger<HomeController> _logger;
private readonly IStreetPerfectClient _Client;
public HomeController(IStreetPerfectClient Client, ILogger<HomeController> logger)
{
  _logger = logger;
  _Client = Client;
}
```

Then make calls to the client.

```
public IActionResult Index()
{
    ViewData["info"] = _Client.GetInfo();
    return View();
}
       
```
 
 The client also contains abstract aspnet controllers allowing you to wire them directly into your own api.
 
```
namespace YourApp.Controllers
{
    [Route("api/my/route/query")]   // your own route
    [ApiExplorerSettings(GroupName = "StreetPerfect")]   // add the StreetPerfect documentation to your own swagger page
    [ApiController]
    public class CA_QueryController : StreetPerfect.Controllers._CA_QueryController
	{
        // all we need to do is pass along your StreetPerfect client instance and a logger
        public CA_QueryController(IStreetPerfectClient Client, ILogger<StreetPerfect.Controllers._CA_QueryController> logger)
        : base(Client, logger)
        {
        }
    }
}
```

Running from a console app without dependancy injection.

```
using StreetPerfect;
using StreetPerfect.Models;

static class Program
{
    public static StreetPerfectClient _Client = new StreetPerfectClient(StreetPerfectClient.defaulConnectionString, false);

    static void Main()
    {
        var req = new caFetchAddressRequest()
        {
            postal_code = "m4w3l4",
            street_number = 365
        };
        var resp = _Client.caFetchAddress(req);
        Console.WriteLine($"caFetchAddress:\r\n{resp.address_line}\r\n{resp.city}, {resp.province}  {resp.postal_code}");
    }
 }
```

