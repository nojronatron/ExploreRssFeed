using ExploreGetRssFeed.Components;
using ExploreGetRssFeed.Data;
using ExploreGetRssFeed.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// add a singleton of ApiHelper to the di container
builder.Services.AddHttpClient();

var app = builder.Build();

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
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
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
