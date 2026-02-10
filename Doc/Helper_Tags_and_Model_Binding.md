# Helper Tags and Model Binding

## Goal
Enhance the existing form by introducing ASP.NET Core helper tags, model binding (without annotations), and ViewBag for feedback messages.

---

## Learning Objectives
By the end of this exercise, you will:

- Use ASP.NET Core Tag Helpers (`asp-for`, `asp-action`)
- Implement model binding without validation attributes
- Utilize ViewBag to display feedback messages after form submission

---

## Step-by-Step Instructions

### Step 1: Create a Model for Form Data
Navigate to the **Models** folder and create a new file named **Subscriber.cs**.

Define a simple class with `Name` and `Email` properties.

**Models/Subscriber.cs**
```csharp
namespace CloudSoft.Models;

public class Subscriber
{
    public string? Name { get; set; }
    public string? Email { get; set; }
}
```

**Information**  
This model will store the form input values and be used for model binding in the controller.

---

### Step 2: Update the Controller
Open **NewsletterController.cs** in the **Controllers** folder.

Modify the `Subscribe` action to accept a `Subscriber` model and store feedback in `ViewBag`.

**Controllers/NewsletterController.cs**
```csharp
using CloudSoft.Models;
using Microsoft.AspNetCore.Mvc;

namespace CloudSoft.Controllers;

public class NewsletterController : Controller
{

    [HttpGet]
    public IActionResult Subscribe()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Subscribe(Subscriber subscriber)
    {
        // Add subscription logic here
        // ...

        // Write to the console
        Console.WriteLine($"New subscription - Name: {subscriber.Name} Email: {subscriber.Email}");

        // Send a message to the user
        ViewBag.Message = $"Thank you for subscribing, {subscriber.Name}!";

        // Return the view
        return View();
    }
}
```

**Information**  
- The `Subscribe` method now accepts a `Subscriber` model, allowing model binding to populate properties automatically.  
- `ViewBag.Message` stores feedback that can be displayed in the view.

---

### Step 3: Update the View to Use Tag Helpers and Display Feedback
Open **Views/Newsletter/Subscribe.cshtml**.

Modify the form to use Tag Helpers and display the feedback message.

**Views/Newsletter/Subscribe.cshtml**
```html
@model CloudSoft.Models.Subscriber

@{
    ViewData["Title"] = "Newsletter Signup";
}

<h2>Newsletter Signup</h2>

@if (ViewBag.Message != null)
{
    <p style="color: green;">@ViewBag.Message</p>
}

<form asp-action="Subscribe" method="post">
    <label asp-for="Name"></label>
    <input asp-for="Name" class="form-control" />

    <label asp-for="Email"></label>
    <input asp-for="Email" class="form-control" />

    <button type="submit">Sign Up</button>
</form>
```

**Information**  
- `@model CloudSoft.Models.Subscriber` tells the view what model to use.  
- `ViewBag.Message` is displayed after a successful submission.  
- The `asp-action` helper automatically sets the form action to **Subscribe**.  
- The `asp-for` Tag Helpers bind input fields directly to the `Subscriber` model.

---

### Step 4: Update the NavBar
The layout defines the shared structure of your application.

Open the **_Layout.cshtml** file in the **Views/Shared** folder.

Add a navigation link to the Newsletter Subscription feature:

**Views/Shared/_Layout.cshtml**
```html
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-controller="Newsletter" asp-action="Subscribe">Subscribe</a>
</li>
```

---

## Final Tests

### Run the Application and Validate Your Work

Start the application:

```bash
dotnet run
```

Open a browser and navigate to:

```
http://localhost:5000/Newsletter/Subscribe
```

Fill out the form and submit it.

Ensure the page reloads and displays a confirmation message.

---

## Shout Out!
Great job implementing Tag Helpers and Model Binding!  
Next, we’ll enhance validation to improve user experience.
