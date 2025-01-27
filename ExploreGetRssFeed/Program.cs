using ExploreGetRssFeed.Components;
using ExploreGetRssFeed.Data;
using ExploreGetRssFeed.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// add .net aspire service defaults
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogging();

// entity framework to persist state
builder.Services.AddDbContextFactory<ExploreGetRssFeedContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString(
            "ExploreGetRssFeedContext") ?? throw new InvalidOperationException(
                "Connection string 'ExploreGetRssFeedContext' not found.")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// inject the data layer interface service
builder.Services.AddScoped<IRssDataAccess, RssDataAccess>();

builder.Services.AddQuickGridEntityFrameworkAdapter();

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// configure weboptimizer for minify, packaging static files
builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/bundle.css", "app.css", "bootstrap/bootstrap.min.css", "ExploreGetRssFeed.styles.css");
});

// if using CORS it must be added prior to add response caching
builder.Services.AddResponseCaching();

// memory cache is volatile and will be reset when the app is restarted
builder.Services.AddMemoryCache();

// get user-configured useragent string from Configuration
string userAgentString = builder.Configuration["HttpClient:UserAgentString"] ?? "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
int httpTimeoutSeconds = 10;

if (int.TryParse(builder.Configuration["HttpClient:TimeoutInt"], out int parsedSeconds))
{
    httpTimeoutSeconds = parsedSeconds;
}

// add a singleton of ApiHelper to the di container
builder.Services.AddHttpClient("RssClient", config =>
{
    config.DefaultRequestHeaders.Add("User-Agent", userAgentString);
    config.DefaultRequestHeaders.Add("Accept", "application/xml");
    config.Timeout = TimeSpan.FromSeconds(httpTimeoutSeconds);
});

var app = builder.Build();

// set .net aspire service endpoints into the pipeline
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

// create the database if it does not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ExploreGetRssFeedContext>();
    var config = services.GetRequiredService<IConfiguration>();
    DbInitializer.Initialize(config, context, app.Environment.IsDevelopment());
}

app.UseHttpsRedirection();

// if using CORS it must be set to use prior to use response caching
app.UseResponseCaching();

// see github.com/ligershark/WebOptimizer for details
app.UseWebOptimizer();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
