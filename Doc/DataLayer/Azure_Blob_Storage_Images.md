# Azure Blob Storage for Images

## Goal

Create an About-page hero section that uses:

- Local image storage in development
- Azure Blob Storage in production

This is implemented through a single image service abstraction and a feature flag.

## Learning Objectives

By the end of this exercise, you will:

- Implement a storage abstraction (`IImageService`)
- Provide local and Azure Blob service implementations
- Bind Azure storage config using the Options Pattern
- Switch storage behavior with a feature flag
- Render a responsive hero section with image background

## Prerequisites

- Existing ASP.NET Core MVC CloudSoft app
- Familiarity with dependency injection and configuration
- Azure Storage account (for production test)
- Basic Razor/CSS knowledge

## Exercise Steps

### Overview

1. Create `IImageService`
2. Add local image implementation
3. Add Azure Blob implementation
4. Add configuration values and feature flags
5. Register services in `Program.cs`
6. Inject service into `HomeController`
7. Update `About.cshtml` hero section
8. Add hero CSS styles
9. Prepare local and cloud images
10. Validate development and production behavior

## Step 1: Create the Image Service Interface

Create `Storage/IImageService.cs`:

```csharp
namespace CloudSoft.Storage;

public interface IImageService
{
    /// <summary>
    /// Gets the URL for an image based on the specified image name.
    /// </summary>
    string GetImageUrl(string imageName);
}
```

Why:

- Keeps controller logic independent of storage backend
- Enables environment-specific implementations without changing calling code

## Step 2: Create Local Image Service Implementation

Create `Storage/LocalImageService.cs`:

```csharp
using Microsoft.AspNetCore.Hosting;

namespace CloudSoft.Storage;

public class LocalImageService : IImageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalImageService(
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetImageUrl(string imageName)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";
        return $"{baseUrl}/images/{imageName}";
    }
}
```

Notes:

- Builds a fully-qualified URL from current request context
- Serves files from `wwwroot/images`

## Step 3: Create Azure Blob Storage Implementation

Create `Configurations/AzureBlobOptions.cs`:

```csharp
namespace CloudSoft.Configurations;

public class AzureBlobOptions
{
    public const string SectionName = "AzureBlob";
    public string ContainerUrl { get; set; } = string.Empty;
}
```

Create `Storage/AzureBlobImageService.cs`:

```csharp
using CloudSoft.Configurations;
using Microsoft.Extensions.Options;

namespace CloudSoft.Storage;

public class AzureBlobImageService : IImageService
{
    private readonly string _blobContainerUrl;

    public AzureBlobImageService(IOptions<AzureBlobOptions> options)
    {
        _blobContainerUrl = options.Value.ContainerUrl;
    }

    public string GetImageUrl(string imageName)
    {
        return $"{_blobContainerUrl}/{imageName}";
    }
}
```

Notes:

- Blob URLs are case-sensitive
- Keep file names consistent across local and cloud (`hero.png`)

## Step 4: Configure Application Settings

Update `appsettings.json`:

```json
{
  "FeatureFlags": {
    "UseMongoDb": false,
    "UseAzureStorage": false
  },
  "AzureBlob": {
    "ContainerUrl": "https://{accountname}.blob.core.windows.net/{container}"
  }
}
```

Update `appsettings.Development.json`:

```json
{
  "FeatureFlags": {
    "UseMongoDb": true,
    "UseAzureStorage": false
  },
  "AzureBlob": {
    "ContainerUrl": "https://localhost:7240/images"
  }
}
```

## Step 5: Register Services in Program.cs

Update `Program.cs`:

```csharp
using CloudSoft.Configurations;
using CloudSoft.Storage;

// Add HttpContextAccessor for local URL generation
builder.Services.AddHttpContextAccessor();

// Bind Azure Blob options
builder.Services.Configure<AzureBlobOptions>(
    builder.Configuration.GetSection(AzureBlobOptions.SectionName));

// Resolve storage implementation from feature flag
bool useAzureStorage = builder.Configuration.GetValue<bool>("FeatureFlags:UseAzureStorage");

if (useAzureStorage)
{
    builder.Services.AddSingleton<IImageService, AzureBlobImageService>();
    Console.WriteLine("Using Azure Blob Storage for images");
}
else
{
    builder.Services.AddSingleton<IImageService, LocalImageService>();
    Console.WriteLine("Using local storage for images");
}
```

## Step 6: Update HomeController

Inject `IImageService` and pass hero URL to view.

`Controllers/HomeController.cs`:

```csharp
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudSoft.Models;
using CloudSoft.Storage;

namespace CloudSoft.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IImageService _imageService;

    public HomeController(ILogger<HomeController> logger, IImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    public IActionResult About()
    {
        ViewData["HeroImageUrl"] = _imageService.GetImageUrl("hero.png");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
```

## Step 7: Create the Hero Section in About View

Update `Views/Home/About.cshtml`:

```cshtml
@{
    ViewData["Title"] = "About";
    var heroImageUrl = ViewData["HeroImageUrl"] as string;
}

<div class="hero-section" style="background-image: url('@heroImageUrl')">
    <div class="hero-content text-white">
        <h1 class="hero-title display-4">Welcome to CloudSoft</h1>
        <div class="hero-text">
            <p class="lead">
                We're passionate about crafting innovative cloud solutions that empower businesses to thrive.
                Explore our services and discover how we can help you reach new heights.
            </p>
        </div>
    </div>
</div>

<div class="container mt-5">
    <div class="row">
        <div class="col-md-6">
            <h2>Our Mission</h2>
            <p>
                At CloudSoft, we believe in the power of cloud computing to transform businesses and drive innovation.
                Our mission is to provide reliable, scalable, and secure cloud solutions that meet the unique needs of our clients.
            </p>
        </div>
        <div class="col-md-6">
            <h2>Our Team</h2>
            <p>
                Our team of cloud experts is dedicated to delivering excellence in every project.
                With years of experience in cloud technologies, we have the knowledge and skills to help you succeed.
            </p>
        </div>
    </div>
</div>
```

## Step 8: Add Hero CSS Styles

Append to `wwwroot/css/site.css`:

```css
/* Hero Section Styles */
.hero-section {
  position: relative;
  height: 500px;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  margin-bottom: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.hero-section::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
}

.hero-content {
  position: relative;
  text-align: center;
  padding: 2rem;
  max-width: 800px;
  z-index: 1;
}

.hero-title {
  font-weight: 700;
  margin-bottom: 1rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
}

.hero-text {
  font-size: 1.2rem;
  max-width: 600px;
  margin: 0 auto;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
}
```

## Step 9: Prepare Local and Cloud Image Assets

Create local folder and add development image:

```bash
mkdir -p wwwroot/images
```

Add file:

- `wwwroot/images/hero.png`

Recommended:

- Development image: ~300-500 KB
- Production image (Blob): higher quality as needed
- Keep file name exactly `hero.png`

## Step 10: Validate the Implementation

### Test Local Mode (Development)

1. Ensure `FeatureFlags:UseAzureStorage` is `false`.
2. Place `hero.png` in `wwwroot/images`.
3. Run app and open About page.

Expected:

- Hero background loads from local URL (example: `https://localhost:7240/images/hero.png`)
- Text overlay remains readable

### Test Azure Blob Mode (Production)

1. Create storage account/container and upload `hero.png`.
2. Set `AzureBlob:ContainerUrl` to real blob container URL.
3. Set `FeatureFlags:UseAzureStorage` to `true`.
4. Run with production config.

Expected:

- Hero background loads from Blob URL (example: `https://{account}.blob.core.windows.net/{container}/hero.png`)
- Same page layout, source switched by configuration only

## Common Issues

- Image not loading: verify `hero.png` name and case.
- Wrong environment behavior: verify `FeatureFlags:UseAzureStorage` in active config file.
- Broken blob URL: ensure `ContainerUrl` has no trailing slash issues and container is accessible.
- Local URL missing host: ensure `AddHttpContextAccessor()` is registered.

## Summary

You implemented a storage-agnostic image delivery pattern for CloudSoft. Development uses local static files, production uses Azure Blob Storage, and the switch is controlled through configuration and dependency injection without changing view/controller logic.
