# Server-Side Validation and Improved Feedback

## Goal
Enhance the form by introducing **server-side validation**, managing a **list of subscribers**, and improving feedback handling using **TempData** and **model-level errors**.

---

## Learning Objectives
By the end of this exercise, you will:

- Implement server-side validation for better security.
- Store subscribers in a list within the controller.
- Use regular expressions to validate email format.
- Prevent duplicate email signups.
- Replace `ViewBag` with TempData for better feedback persistence.
- Use `ModelState.AddModelError` to improve error handling.

---

## Step-by-Step Instructions

### Step 1: Update the Model with Server-Side Validation
Open `Subscriber.cs` in the `Models` folder.

Improve email validation using **regular expressions**.

**Models/Subscriber.cs**
```csharp
using System.ComponentModel.DataAnnotations;

namespace CloudSoft.Models;

public class Subscriber
{
    [Required]
    [StringLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Missing top level domain")]
    public string? Email { get; set; }
}
```

**Information**
- The `RegularExpression` attribute enforces stricter email format validation.
- This prevents malformed or suspicious email input.

---

### Step 2: Update the Controller to Store Subscribers and Prevent Duplicates
Open `NewsletterController.cs`.

Modify the `Subscribe` method to:
- validate the model (server-side),
- check for duplicate emails,
- store subscribers in memory,
- use TempData for success feedback,
- use PRG (**Post-Redirect-Get**) to avoid duplicate form submission on refresh.

**Controllers/NewsletterController.cs**
```csharp
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
        TempData["SuccessMessage"] =
            $"Thank you for subscribing, {subscriber.Name}! You will receive our newsletter at {subscriber.Email}";

        // Return the view (using the POST-REDIRECT-GET pattern)
        return RedirectToAction(nameof(Subscribe)); // use nameof() to find the action by name during compile time
    }
}
```

**Information**
- `_subscribers` list stores all registered users in memory.
- Checks if the email already exists before allowing submission.
- Uses `ModelState.AddModelError` for field-specific error messages.
- TempData replaces ViewBag for better feedback persistence across redirects.

---

### Step 3: Update the View to Display Errors and TempData Feedback
Open `Views/Newsletter/Subscribe.cshtml`.

Modify the form to:
- show server-side validation summary,
- show TempData success message,
- show field-level error messages.

**Views/Newsletter/Subscribe.cshtml**
```cshtml
@model CloudSoft.Models.Subscriber

@{
    ViewData["Title"] = "Newsletter Signup";
}

<h2>Newsletter Signup</h2>

<!-- Display validation summary for model-level errors -->
@if (!ViewData.ModelState.IsValid)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        @Html.ValidationSummary(false, null, new { @class = "text-danger" })
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">
            <i class="fas fa-times"></i>
        </button>
    </div>
}

<!-- Display message from the TempData sent by the controller -->
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        @TempData["SuccessMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close">
            <i class="fas fa-times"></i>
        </button>
    </div>
}

<form asp-action="Subscribe" method="post">
    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Email"></label>
        <input asp-for="Email" class="form-control" />
        <span asp-validation-for="Email" class="text-danger"></span>
    </div>

    <div class="mt-3">
        <button type="submit" class="btn btn-primary">Sign Up</button>
    </div>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**Information**
- TempData message is displayed when redirecting after a successful submission.
- `asp-validation-for` displays field-specific validation errors.
- `_ValidationScriptsPartial` enables client-side validation.

---

## Final Tests

### Run the Application and Validate Your Work
Start the application:
```bash
dotnet run
```

Open a browser and navigate to:
```text
http://localhost:5000/Newsletter/Subscribe
```

Try the following:
1. Submit invalid or empty input and confirm error messages appear.
2. Register with the same email twice and confirm the duplicate warning appears.
3. Submit valid data and verify the success feedback persists correctly using TempData (after redirect).

---

## Shout Out!
Awesome work! You’ve implemented server-side validation, handled duplicate entries, and improved feedback management.
