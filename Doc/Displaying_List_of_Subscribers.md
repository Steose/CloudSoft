# 📋 Displaying a List of Subscribers

## 🎯 Goal
Enhance the application by adding a new view to list all subscribers and updating the navigation bar to include a button linking to the new page.

---

## 📚 Learning Objectives
By the end of this exercise, you will be able to:

- Create a new view to display a list of subscribers  
- Modify the controller to pass subscriber data to the view  
- Update the navigation bar to provide access to the new page  

---

## 🧭 Step-by-Step Instructions

---

## ✅ Step 1: Update the Controller to Provide Subscriber Data

Open **NewsletterController.cs** and add a new action method that returns the list of subscribers to the view.

**Path:** `Controllers/NewsletterController.cs`

```csharp
[HttpGet]
public IActionResult Subscribers()
{
    return View(_subscribers);
}
```

### ℹ️ Information
- The `Subscribers()` action returns the list of subscribers to the view.
- The list is retrieved from the in-memory static list.

---

## ✅ Step 2: Create the Subscriber List View

Navigate to the **Views/Newsletter/** folder and create a new file named **Subscribers.cshtml**.

**Path:** `Views/Newsletter/Subscribers.cshtml`

```cshtml
@model List<CloudSoft.Models.Subscriber>

@{
    ViewData["Title"] = "Subscriber List";
}

<h2>Subscriber List</h2>

@if (Model.Count == 0)
{
    <p>No subscribers yet.</p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Name</th>
                <th>Email</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var subscriber in Model)
            {
                <tr>
                    <td>@subscriber.Name</td>
                    <td>@subscriber.Email</td>
                </tr>
            }
        </tbody>
    </table>
}
```

### ℹ️ Information
- Displays all subscribers in a table.
- If no subscribers exist, a fallback message is shown.

---

## ✅ Step 3: Add a Navigation Button to the Navbar

Open **_Layout.cshtml** and add a new navigation link to the Subscriber List page.

**Path:** `Views/Shared/_Layout.cshtml`

```cshtml
<li class="nav-item">
    <a class="nav-link text-dark"
       asp-area=""
       asp-controller="Newsletter"
       asp-action="Subscribers">
       Subscribers
    </a>
</li>
```

### ℹ️ Information
- Adds a navigation button to access the Subscriber List page.

---

## 🧪 Final Tests – Run and Validate

Start the application:

```bash
dotnet run
```

Open a browser and navigate to:

```
http://localhost:5000/Newsletter/Subscribe
```

### ✔️ Validation Checklist
- Subscribe with multiple names and emails
- Click **Subscribers** in the navbar
- Ensure all submitted data appears correctly in the table

---

## 🎉 Shout Out!

Great job!  
You’ve successfully added a subscriber list page and improved the user experience with clear navigation. 🚀
