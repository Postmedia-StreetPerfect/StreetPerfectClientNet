# StreetPerfectClient for .Net
C# XPC Client for StreetPerfect native AND HTTP access

The easiest way to use the C# client is to add it as a singleton service in your ConfigureServices() aspnet function.

```C#
var sp_connectionString = Configuration.GetConnectionString("StreetPerfectServer");
var sp_debug = Configuration.GetSection("AppSettings:Debug").Value == "True";
services.AddSingleton(typeof(IStreetPerfectClient), new StreetPerfectClient(sp_connectionString, sp_debug));
```

You can then inject the service into any controller.

```C#
private readonly ILogger<HomeController> _logger;
private readonly IStreetPerfectClient _Client;
public HomeController(IStreetPerfectClient Client, ILogger<HomeController> logger)
{
  _logger = logger;
  _Client = Client;
}
```

Then make calls to the client.

```C#
public IActionResult Index()
{
    ViewData["info"] = _Client.GetInfo();
    return View();
}
       
```

There is also an HTTP Client for accessing api.streetperfect.com

```C#
// setup the SP REST client at Program startup
builder.Services.AddStreetPerfectClient(c =>
{
	c.ClientId = appSettings.SPRestConfig?.ClientId; // YOUR api.StreetPerfect.com client ID
	c.ClientSecret = appSettings.SPRestConfig?.ClientSecret; //YOUR api.StreetPerfect.com API key
	c.UseDevEndpoint = true; // use api.streetperfect or apidev.streetperfect
});
```

Inject the HTTP client like you would the native client

```C#
private readonly ILogger<HomeController> _logger;
private readonly IStreetPerfectRestClient _Client;
public HomeController(IStreetPerfectRestClient Client, ILogger<HomeController> logger)
{
  _logger = logger;
  _Client = Client;
}
```
 
And use the HTTP client (alomst) exactely like the native client - all HTTP client calls are asynchronous

```C#
public async Task<IActionResult> Index()
{
    ViewData["info"] = await _Client.GetInfo();
    return View();
}
       
```
 
You can also use the HTTP client to connect to your own HTTP enabled StreetPerfect server.
Note that the HTTP client requires the ReFit nuget package - which makes http rest client creation & use almost trivial.

```C#
builder.Services.AddRefitClient<IStreetPerfectRestClient>().ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri("Your own private server url");
	});
```
 
 
 
 The client also contains abstract aspnet controllers allowing you to wire them directly into your own api. This also allows you to add the same Swagger pages as our API site.
 
```C#
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

Running from a console app without dependency injection.

```C#
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

