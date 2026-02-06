# Implement Unsubscribe Functionality (ASP.NET Core MVC)

## Goal
Add the ability for users to **unsubscribe** from the newsletter and enhance the subscriber list with **action buttons**.

---

## Learning Objectives
By the end of this exercise, you will:

- Add an **unsubscribe** action to the controller
- Implement **confirmation dialogs** for destructive actions
- Add **action buttons** to table rows
- Handle **POST** requests securely with **anti-forgery tokens**

---

## Step-by-Step Instructions

## Step 1: Update the Controller with `Unsubscribe` Action

Open: `Controllers/NewsletterController.cs`

Add a new action method for handling unsubscribe requests:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Unsubscribe(string email)
{
    var subscriber = _subscribers.FirstOrDefault(s => s.Email == email);
    if (subscriber != null)
    {
        _subscribers.Remove(subscriber);
        TempData["SuccessMessage"] = $"Successfully unsubscribed {email} from the newsletter.";
    }
    return RedirectToAction(nameof(Subscribers));
}
```

### Information

- Uses **`[HttpPost]`** to prevent accidental unsubscribes via **GET** requests
- Removes subscriber from list based on **email**
- Provides feedback via **`TempData`**
- Redirects back to **subscriber list**

### Why `[ValidateAntiForgeryToken]` matters (CSRF protection)

The **`[ValidateAntiForgeryToken]`** attribute ensures that:

- The form submission includes a **valid anti-forgery token**
- Protects against **Cross-Site Request Forgery (CSRF)** attacks
- Returns **400 Bad Request** if the token is **missing or invalid**

The client-side token in the View (**`@Html.AntiForgeryToken()`**) works together with this server-side validation to provide security against CSRF attacks.

---

## Step 2: Update the Subscriber List View

Open: `Views/Newsletter/Subscribers.cshtml`

Add an unsubscribe button and confirmation dialog:

```cshtml
@model List<CloudSoft.Models.Subscriber>

@{
    ViewData["Title"] = "Subscriber List";
}

<h2>Subscriber List</h2>

<!-- Display message from the TempData sent by the controller -->
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        @TempData["SuccessMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"><i class="fas fa-times"></i></button>
    </div>
}

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
                <th></th>
            </tr>
        </thead>
        <tbody>
            @foreach (var subscriber in Model)
            {
                <tr>
                    <td>@subscriber.Name</td>
                    <td>@subscriber.Email</td>
                    <td>
                        <form asp-action="Unsubscribe" method="post" class="d-inline"
                              onsubmit="return confirm('Are you sure you want to unsubscribe @subscriber.Email?');">
                            @Html.AntiForgeryToken()
                            <input type="hidden" name="email" value="@subscriber.Email" />
                            <button type="submit" class="btn btn-outline-danger btn-sm">
                                <i class="fas fa-user-minus me-1"></i>Unsubscribe
                            </button>
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
```

### Information

- Uses a `<form>` for secure **POST** requests
- Includes anti-forgery token: **`@Html.AntiForgeryToken()`**
- Adds confirmation dialog using JavaScript:

```html
onsubmit="return confirm('Are you sure you want to unsubscribe @subscriber.Email?');"
```

---

## Final Tests

## 1) Functionality Test

1. Subscribe a new user
2. View the subscriber list
3. Click **Unsubscribe**
4. Verify confirmation dialog appears
5. Confirm unsubscribe

### Expected Results

- Confirmation dialog shows the **correct email**
- Success message appears after unsubscribe
- User is removed from list
- List updates immediately

---

## 2) Security Test (CSRF protection)

> ✅ This test demonstrates why `[ValidateAntiForgeryToken]` is important.

### A) Temporarily comment out `[ValidateAntiForgeryToken]`

1. Add subscriber: `test@example.com`
2. Go to the subscriber list page
3. Open a terminal and run:

```bash
curl -X POST http://localhost:5220/Newsletter/Unsubscribe -d "email=test@example.com"
```

4. Refresh the subscriber list page and verify **`test@example.com` is gone**

✅ This shows that without anti-forgery validation, a forged POST can succeed.

### B) Restore `[ValidateAntiForgeryToken]` and repeat

1. Remove the comment (enable `[ValidateAntiForgeryToken]` again)
2. Add subscriber again: `test@example.com`
3. Run the same `curl` command again:

```bash
curl -X POST http://localhost:5220/Newsletter/Unsubscribe -d "email=test@example.com"
```

4. Refresh the subscriber list page and verify **`test@example.com` is still there**

✅ With anti-forgery enabled, the request should fail (commonly **400 Bad Request**) because the token is missing.

**Note:**  
`-d "email=test@example.com"` specifies the data sent in the body of the POST request.

---

## Exercise Done! 🎉
You’ve successfully added unsubscribe functionality with proper security measures and user feedback!
