# 2. Create A Form With Basic HTML

## Goal
Create a basic HTML form that collects name and email and submits it via POST.  
This step introduces fundamental form handling in an ASP.NET Core MVC web application without using advanced features like model binding or validation.

---

## Learning Objectives
By the end of this exercise, you will:

- Understand how to create an HTML form with basic input fields.
- Learn how to submit form data using an HTTP POST request.
- Implement a controller to handle form display and submission.
- Display submitted data in the application console.

---

## Step-by-Step Instructions

### Step 1: Create a Controller

Navigate to the **Controllers** folder and create a new file named:

```
NewsletterController.cs
```

Add the following code:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace CloudSoft.Controllers;

public class NewsletterController : Controller
{
    // GET: /Newsletter/Subscribe
    public IActionResult Subscribe()
    {
        return View();
    }

    // POST: /Newsletter/Subscribe
    [HttpPost]
    public IActionResult Subscribe(string name, string email)
    {
        // Add subscription logic here
        // ...

        // Write to the console
        Console.WriteLine($"New subscription - Name: {name} Email: {email}");

        // Send a message to the user
        return Content($"Thank you {name} for subscribing to our newsletter!");
    }
}
```

#### Information
- The **GET** `Subscribe()` method renders the page with the form.
- The **POST** `Subscribe()` method handles the form submission and returns a simple message.

---

### Step 2: Create the Form View

Navigate to:

```
Views/Newsletter/
```

Create a new file named:

```
Subscribe.cshtml
```

Add the following HTML:

```html
@{
    ViewData["Title"] = "Sign up for our newsletter";
}

<h2>Sign up for our newsletter</h2>

<form action="/Newsletter/Subscribe" method="post">
    <label for="name">Name:</label>
    <input type="text" id="name" name="name">

    <label for="email">Email:</label>
    <input type="email" id="email" name="email">

    <button type="submit">Sign Up</button>
</form>
```

#### Information
- The form submits data to `/Newsletter/Subscribe` using **HTTP POST**.
- The submitted fields are:
  - `name`: The user's name
  - `email`: The user's email address
- The **name attribute** of each input field is what ASP.NET Core uses to receive values in the controller.

---

## Final Tests

### Step 1: Run the Application

Start the application:

```bash
dotnet run
```

Open your browser and navigate to:

```
http://localhost:<PORT>/Newsletter/Subscribe
```

(Replace `<PORT>` with the port your application is using.)

**Expected Result**
- You should see the newsletter signup form.

---

### Step 2: Fill Out and Submit the Form

1. Enter a name and an **incorrect email** (without `@`).
2. Click **Sign Up**.
3. Enter a name and a **correct email**.
4. Click **Sign Up** again.

**Expected Result**
- Incorrect email should show an informational browser message.
- Correct submission should display:

```
Thank you [entered name] for subscribing to our newsletter!
```

- In the application console, you should see:

```
New subscription - Name: [Your Name], Email: [Your Email]
```

---

🎉 **You have a working HTML form in ASP.NET Core MVC!**
