# Identity Project

This project implements a custom Identity solution using Microsoft ASP.NET. The application allows users to:

- **Register and Manage Accounts:** Users can create an account, update their personal information, and upload a profile picture.
- **Password Recovery:** If a user forgets their password, they can reset it via their registered email address.
- **Role Management:** The application includes two roles—Admin and User. Admins have elevated privileges, allowing them to add, edit, or
- remove users from the system.



## Technologies:
1. **ASP.NET**
2. **JavaScript & jQuery**
3. **Bootstrap 5**


## Prerequisites

Before running this project, ensure that you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.NET and web development workload installed

## Configuration

### 1. **Configure `appsettings.json`**

Before running the project, you need to configure the `appsettings.json` file with your own settings. 

#### **`appsettings.json` fields to configure:**

- **Email Settings:**
"EmailSettings": {
    "FromEmail": "your email",
    "FromPassword": "your app password",//App password
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587
  },

- **SQL Server Connection String:**
  - `ConnectionStrings:DefaultConnection`: The connection string for the SQL Server database. Update the `User ID` and `Password` fields with your SQL Server credentials.

#### **Example Configuration:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;MultipleActiveResultSets=true;Encrypt=false"
  },
  "Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "your_email@gmail.com",
    "SenderName": "Your Name",
    "AppPassword": "your_app_password"
  }
}
```


### 2. **Generate an App Password**

If you are using Gmail or any other email provider with two-factor authentication (2FA), you'll need to generate an app password to send emails through the application.

**To generate an app password in Gmail:**

1. Go to your Google Account.
2. Navigate to `Security`.
3. Under `Signing in to Google`, select `App passwords`. (You might need to sign in again.)
4. Select `Select app` and choose `Other (Custom name)`.
5. Enter a name (e.g., "Identity Project") and click `Generate`.
6. Copy the generated app password and paste it into the `AppPassword` field in `appsettings.json`.

### 3. Update the Database

### Summary:

- The `README.md` file gives a clear description of the project.
- It provides instructions on how to configure the `appsettings.json` file, including how to generate an app password for email settings.
- It explains how to update the database and run the project.

You can copy this content into your `README.md` file in your project's root directory.
