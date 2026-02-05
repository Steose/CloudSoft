# Hello World MVC Application

## 🎯 Goal
Create a basic **ASP.NET Core MVC** application, run it locally, and prepare it for version control with Git.

---

## 📚 Learning Objectives
By the end of this exercise, you will:

- Create a new ASP.NET Core MVC project  
- Understand the basic project structure of an MVC application  
- Run a web application locally  
- Initialize Git for version control  
- Publish your code to GitHub  

---

## 🛠 Step-by-Step Instructions

### Step 1: Create a New ASP.NET Core MVC Application

1. Open a terminal or command prompt  
2. Navigate to your desired project location  
3. Run the following command:

```bash
dotnet new mvc -n CloudSoft
```

**Information**
- `dotnet new mvc` creates a new ASP.NET Core MVC project  
- `-n CloudSoft` sets the project name to **CloudSoft**  
- This creates folders for **Controllers**, **Models**, and **Views**  

4. Navigate into the project folder:

```bash
cd CloudSoft
```

---

### Step 2: Explore the Project Structure

Key folders and files:

- **Controllers/** – Handles HTTP requests  
- **Models/** – Contains data and business logic  
- **Views/** – Razor UI files (`.cshtml`)  
- **wwwroot/** – Static files (CSS, JS, images)  
- **Program.cs** – Application entry point  
- **CloudSoft.csproj** – Project configuration and dependencies  

**MVC Pattern Overview**
- **Model**: Data and logic  
- **View**: User interface  
- **Controller**: Handles user input and responses  

---

### Step 3: Run the Application Locally

Start the application:

```bash
dotnet run
```

Open your browser and navigate to:

- `https://localhost:7240`  
- or `http://localhost:5000`  

> ⚠️ Port numbers may vary

**Verification**
- You should see the default ASP.NET Core welcome page.

---

### Step 4: Initialize Git for Version Control

1. Create a `.gitignore` file:

```bash
dotnet new gitignore
```

2. Initialize Git:

```bash
git init
```

3. Stage all files:

```bash
git add .
```

4. Create your first commit:

```bash
git commit -m "Initial commit: Create ASP.NET Core MVC application"
```

**Information**
- Git tracks changes to your code  
- `.gitignore` excludes unnecessary files  
- Commits represent snapshots of your project  

---

### Step 5: Publish to GitHub

1. Create a new repository on GitHub:
   - Click **+ → New repository**
   - Name it **CloudSoft**
   - Do **not** initialize with README, .gitignore, or license

2. Link and push your repository:

```bash
git remote add origin https://github.com/YourUsername/CloudSoft.git
git branch -M main
git push -u origin main
```

> Replace `YourUsername` with your GitHub username

---

## ✅ Final Tests

### Test 1: Verify Your Web Application
- Run:

```bash
dotnet run
```

- Open the application URL  
- Verify:
  - Homepage loads correctly  
  - Privacy page is accessible  

**Expected Result**
- Application runs without errors  
- Welcome page appears  

---

### Test 2: Verify Your GitHub Repository
- Open your GitHub repository  
- Verify:
  - All project files are present  
  - Commit message is visible  
  - Repository structure matches local project  

---

🎉 **Congratulations!**  
You’ve successfully created, run, version-controlled, and published your first ASP.NET Core MVC application.
