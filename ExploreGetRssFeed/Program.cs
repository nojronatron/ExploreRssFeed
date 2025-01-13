using ExploreGetRssFeed.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogging();

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

app.UseHttpsRedirection();

// if using CORS it must be set to use prior to use response caching
app.UseResponseCaching();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
