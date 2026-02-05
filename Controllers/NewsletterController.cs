using System.Text.RegularExpressions;
using CloudSoft.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudSoft.Controllers;

public class NewsletterController : Controller
{
    // Create a "database" of subscribers for demonstration purposes
    private static List<Subscriber> _subscribers = [];

    [HttpGet]
    public IActionResult Subscribe()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Subscribe(Subscriber subscriber)
    {
        // Validate the model
        if (!ModelState.IsValid)
        {
            return View(subscriber);
        }

        // Check if the email is already subscribed and return a general model level error
        if (_subscribers.Any(s => s.Email == subscriber.Email))
        {
            ModelState.AddModelError("Email", "The email is already subscribed. Please use a different email.");
            return View(subscriber);
        }

        // Add the subscriber to the list
        _subscribers.Add(subscriber);

        // Write to the console
        Console.WriteLine($"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}");

        // Send a message to the user
        TempData["SuccessMessage"] = $"Thank you for subscribing, {subscriber.Name}! You will receive our newsletter at {subscriber.Email}";

        // Return the view (using the POST-REDIRECT-GET pattern)
        return RedirectToAction(nameof(Subscribe));  // use nameof() to find the action by name during compile time
    }
}
