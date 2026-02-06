# Landing Page with Hero Section (Bootstrap + ASP.NET Core MVC)

## Goal
Create an engaging landing page with a hero section and a call-to-action button that directs users to the newsletter subscription page.

## Learning Objectives
By the end of this exercise, you will be able to:

- Create a hero section using Bootstrap
- Implement call-to-action buttons
- Use Bootstrap’s spacing utilities
- Apply responsive text classes
- Create gradient backgrounds

---

## Step-by-Step Instructions

### Step 1: Create the Landing Page
1. Open: `Views/Home/Index.cshtml`
2. Replace the content with a hero section and call-to-action button

**File:** `Views/Home/Index.cshtml`

```cshtml
@{
    ViewData["Title"] = "Welcome to CloudSoft";
}

<div class="container-fluid px-0">
    <!-- Hero Section -->
    <div class="hero-gradient text-white py-5">
        <div class="container py-5">
            <div class="row align-items-center g-5">
                <div class="col-lg-6 mx-auto text-center">
                    <h1 class="display-3 fw-bold mb-3">Stay Connected with CloudSoft</h1>
                    <p class="lead mb-4">
                        Get the latest updates, news, and special offers delivered directly to your inbox.
                        Join our newsletter community today!
                    </p>
                    <div class="d-grid gap-2 d-md-flex justify-content-center">
                        <a asp-controller="Newsletter" asp-action="Subscribe"
                           class="btn btn-light btn-lg px-4 shadow">
                            <i class="fas fa-envelope me-2"></i>Subscribe Now
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

#### Information (What’s going on?)
**Container Classes**
- `container-fluid px-0` removes horizontal padding for full-width sections
- `container` centers content with a max-width

**Background & Text**
- Custom gradient (`hero-gradient`) creates a modern look
- `text-white` ensures readability on a darker background

**Spacing Utilities**
- `py-5` adds vertical padding
- `mb-3` / `mb-4` adds bottom margin
- `g-5` adds gutters between columns

**Typography**
- `display-3` makes the heading larger
- `lead` styles the paragraph as an intro

**Flex/Grid Utilities**
- `d-grid d-md-flex` changes button layout at the `md` breakpoint
- `align-items-center` vertically aligns the row content

**Components**
- `btn-lg` makes the button larger
- `shadow` adds depth for a “raised” look

---

### Step 2: Add Custom CSS Styles
1. Open: `wwwroot/css/site.css`
2. Add the gradient class (keep your existing CSS; add this where appropriate)

**File:** `wwwroot/css/site.css`

```css
/* ... existing styles ... */

.hero-gradient {
    background: linear-gradient(135deg, var(--bs-primary) 0%, var(--bs-info) 100%);
}

/* ... existing styles ... */
```

#### Information
**Gradient Background**
- Uses Bootstrap theme color variables: `--bs-primary` and `--bs-info`
- Keeps styling consistent with your selected theme
- `135deg` creates a diagonal gradient for visual interest

---

### Step 3: Style the About Page
The About page keeps the same card-based layout and styling patterns as the other pages while introducing an image and feature highlights.

1. Add an image to: `wwwroot/images/`
2. Name it: `about-header.jpg`
3. Create the page content below

**File:** `Views/Home/About.cshtml`

```cshtml
@{
    ViewData["Title"] = "About CloudSoft";
}

<div class="container py-4">
    <div class="row justify-content-center">
        <div class="col-12 col-lg-8">
            <div class="card shadow">
                <img src="~/images/about-header.jpg" class="card-img-top" alt="CloudSoft Office">
                <div class="card-body">
                    <h2 class="card-title h3 mb-4">About CloudSoft</h2>
                    <div class="row g-4">
                        <div class="col-md-6">
                            <h3 class="h5 mb-3">Our Mission</h3>
                            <p>At CloudSoft, we're passionate about delivering cutting-edge cloud solutions that help businesses thrive in the digital age.</p>
                        </div>
                        <div class="col-md-6">
                            <h3 class="h5 mb-3">Our Vision</h3>
                            <p>We envision a future where every business, regardless of size, can harness the power of cloud technology to achieve their goals.</p>
                        </div>
                        <div class="col-12">
                            <div class="bg-light p-4 rounded">
                                <h3 class="h5 mb-3">Why Choose Us?</h3>
                                <div class="row g-3">
                                    <div class="col-md-4">
                                        <div class="d-flex align-items-center">
                                            <i class="fas fa-cloud-upload-alt text-primary fa-2x me-3"></i>
                                            <div>
                                                <h4 class="h6 mb-1">Cloud Expertise</h4>
                                                <p class="small mb-0">10+ years experience</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="d-flex align-items-center">
                                            <i class="fas fa-users text-primary fa-2x me-3"></i>
                                            <div>
                                                <h4 class="h6 mb-1">Support Team</h4>
                                                <p class="small mb-0">24/7 availability</p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="d-flex align-items-center">
                                            <i class="fas fa-shield-alt text-primary fa-2x me-3"></i>
                                            <div>
                                                <h4 class="h6 mb-1">Security First</h4>
                                                <p class="small mb-0">Enterprise-grade</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div><!-- /.row -->
                </div><!-- /.card-body -->
            </div><!-- /.card -->
        </div><!-- /.col -->
    </div><!-- /.row -->
</div><!-- /.container -->
```

#### Information
**Image Handling**
- `card-img-top` properly sizes the header image
- Place images in `wwwroot/images` and reference them with `~/images/...`

**Grid Layout**
- Uses nested rows and columns for more complex layouts
- `g-4` adds spacing between grid items

**Content Cards**
- `bg-light` creates subtle section backgrounds
- `rounded` adds consistent border radius

**Icons & Typography**
- Font Awesome icons with consistent sizing and Bootstrap color classes
- Hierarchical headings with Bootstrap typography classes (`h3`, `h5`, `h6`)

**Responsive Behavior**
- Single column on mobile
- Multi-column layout on larger screens

---

## Final Tests

### Visual Inspection
- Resize the browser window and confirm responsive behavior

**Expected Results**
- Hero section fills the viewport width
- Smooth gradient background
- Text remains readable and centered

### Functionality Test
- Click the **Subscribe Now** button
- Verify navigation to the newsletter subscription page

**Expected Results**
- Button navigates to the correct page
- Links work correctly
- Smooth transitions between pages
- Consistent styling across pages

---

## Exercise Done ✅
You’ve created an engaging landing page that draws users to your newsletter subscription!
