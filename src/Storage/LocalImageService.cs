using Microsoft.AspNetCore.Hosting;

namespace CloudSoft.Storage;

public class LocalImageService : IImageService
{
    // Holds environment info
    private readonly IWebHostEnvironment _webHostEnvironment; 
    
    // Gives access to current HTTP request
    private readonly IHttpContextAccessor _httpContextAccessor; 

    public LocalImageService(
        IWebHostEnvironment webHostEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _webHostEnvironment = webHostEnvironment; // Save environment dependency
        _httpContextAccessor = httpContextAccessor; // Save HttpContext dependency
    }

    public string GetImageUrl(string imageName) // Method to build full image URL
    {
        var request = _httpContextAccessor.HttpContext?.Request; // Get current request
        var baseUrl = $"{request?.Scheme}://{request?.Host}"; // Build base URL like https://localhost:5001

        return $"{baseUrl}/images/{imageName}"; // Return full image URL
    }
}