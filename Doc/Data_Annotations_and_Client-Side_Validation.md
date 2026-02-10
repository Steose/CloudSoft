# Data Annotations and Client-Side Validation

## Goal
Enhance the existing form by introducing data annotations for client-side validation to improve user experience and enforce input constraints.

## Learning Objectives
By the end of this exercise, you will:

- Use data annotations (`[Required]`, `[EmailAddress]`, `[StringLength]`)
- Enable client-side validation using ASP.NET Core built-in validation
- Improve form feedback with validation messages

---

## Step-by-Step Instructions

### Step 1: Update the Model with Data Annotations
Open `Subscriber.cs` in the `Models` folder.  
Add validation attributes to enforce required fields and format constraints.

**File:** `Models/Subscriber.cs`

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
    public string? Email { get; set; }
}
```

#### Information
- `[Required]` ensures the field must be filled.
- `[StringLength]` limits the length of the `Name` field.
- `[EmailAddress]` ensures a valid email format.

---

### Step 2: Update the View to Display Validation Messages
Open `Views/Newsletter/Subscribe.cshtml`.  
Add validation messages for form fields.

**File:** `Views/Newsletter/Subscribe.cshtml`

```cshtml
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

    <button type="submit">Sign Up</button>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

#### Information
- `asp-validation-for` displays validation errors next to the corresponding field.
- `_ValidationScriptsPartial` enables client-side validation using jQuery validation scripts (the default setup in ASP.NET Core MVC templates).

---

## Final Tests

### Run the Application and Validate Your Work

1. Start the application:

```bash
dotnet run
```

2. Open a browser and navigate to:

```
http://localhost:5000/Newsletter/Subscribe
```

3. Try submitting the form with missing or invalid data.
   - Ensure error messages appear and prevent submission.

4. Fill in valid data and confirm successful submission.

---

## Shout Out!
Fantastic job! You’ve successfully added data validation and client-side validation 🎉
