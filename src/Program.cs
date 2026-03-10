using CloudSoft.Models;
using CloudSoft.Services;
using CloudSoft.Repositories;
using CloudSoft.Configurations;
using CloudSoft.Storage;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Ensure static web assets (e.g., CloudSoft.styles.css from CSS isolation) are available
// when running from source/non-published output across environments.
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Check if MongoDB should be used (default to false if not specified)
bool useMongoDb = builder.Configuration.GetValue<bool>("FeatureFlags:UseMongoDb");

if (useMongoDb)
{
    // Configure MongoDB options
    builder.Services.Configure<MongoDbOptions>(
        builder.Configuration.GetSection(MongoDbOptions.SectionName));

    // Configure MongoDB client
    builder.Services.AddSingleton<IMongoClient>(serviceProvider => {
        var mongoDbOptions = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>()
            ?? throw new InvalidOperationException(
                $"Missing '{MongoDbOptions.SectionName}' configuration section.");

        ValidateMongoDbOptions(mongoDbOptions);
        return new MongoClient(mongoDbOptions.ConnectionString);
    });

    // Configure MongoDB collection
    builder.Services.AddSingleton(serviceProvider => {
        var mongoDbOptions = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>()
            ?? throw new InvalidOperationException(
                $"Missing '{MongoDbOptions.SectionName}' configuration section.");

        ValidateMongoDbOptions(mongoDbOptions);
        var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
        var database = mongoClient.GetDatabase(mongoDbOptions.DatabaseName);
        return database.GetCollection<Subscriber>(mongoDbOptions.SubscribersCollectionName);
    });

    // Register MongoDB repository
    builder.Services.AddSingleton<ISubscriberRepository, MongoDbSubscriberRepository>();

    Console.WriteLine("Using MongoDB repository");
}
else
{
    // Register in-memory repository as fallback
    builder.Services.AddSingleton<ISubscriberRepository, InMemorySubscriberRepository>();

    Console.WriteLine("Using in-memory repository");
}

// Add HttpContextAccessor for URL generation
builder.Services.AddHttpContextAccessor();

// Configure Azure Blob options
builder.Services.Configure<AzureBlobOptions>(
    builder.Configuration.GetSection(AzureBlobOptions.SectionName));

// Check if Azure Storage should be used
bool useAzureStorage = builder.Configuration.GetValue<bool>("FeatureFlags:UseAzureStorage");

if (useAzureStorage)
{
    // Register Azure Blob Storage image service for production
    builder.Services.AddSingleton<IImageService, AzureBlobImageService>();
    Console.WriteLine("Using Azure Blob Storage for images");
}
else
{
    // Register local image service for development
    builder.Services.AddSingleton<IImageService, LocalImageService>();
    Console.WriteLine("Using local storage for images");
}

// Register service (depends on repository)
builder.Services.AddScoped<INewsletterService, NewsletterService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

static void ValidateMongoDbOptions(MongoDbOptions options)
{
    if (string.IsNullOrWhiteSpace(options.ConnectionString))
    {
        throw new InvalidOperationException(
            "MongoDB is enabled, but 'MongoDb:ConnectionString' is empty. " +
            "Set it with User Secrets for Development or the 'MongoDb__ConnectionString' environment variable for Production.");
    }

    if (ContainsPlaceholder(options.ConnectionString))
    {
        throw new InvalidOperationException(
            "MongoDB is enabled, but 'MongoDb:ConnectionString' still contains template placeholders such as '{hostname}' or '{port}'. " +
            "Replace it with a real MongoDB/Cosmos DB connection string. " +
            "For Development use 'dotnet user-secrets set \"MongoDb:ConnectionString\" \"...\"'. " +
            "For Production use the 'MongoDb__ConnectionString' environment variable.");
    }
}

static bool ContainsPlaceholder(string connectionString)
{
    return connectionString.Contains("{", StringComparison.Ordinal)
        || connectionString.Contains("}", StringComparison.Ordinal)
        || connectionString.Contains("<", StringComparison.Ordinal)
        || connectionString.Contains(">", StringComparison.Ordinal);
}
