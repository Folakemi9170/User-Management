Project Overview
This system is designed to address the fundamental needs of an organization by managing the activities digitally. 
The system streamlines internal operations such as employee management, role assignment, department coordination improving efficiency and data accessibility across all departments.

Objectives
Digitize and automate organizational processes.
Provide secure user access based on role and permissions.
Improve communication between management and staff.

Key Features
Onion Architecture (Domain → Application → Infrastructure → Presentation)
Authentication & Authorization with JWT
CRUD operations
Entity Framework Core for database access
Middleware Components
Global Error Handling

Technologies Used
C#
ASP.NET Core 9.0
Entity Framework Core
BCrypt
JSON Web Token
SQL Server (or your configured DB)
AutoMapper
MailKit (for email services)
Serilog

Database Design: Tables in the database include:
Users
Roles
Permissions
Departments
Employees
Products
Job Titles
Permission
Product Category
Product Type
Product Status
Customers

System Flow:
The User Management system follows a well-structured process that begins the moment a new user lands on the application. 
Each step ensures secure onboarding, data integrity, and controlled access to system resources.

i.	Signup Page (First Landing Page)
When a new user visits the system for the first time, they are presented with a signup page.
The user provides a valid email address and password, which are securely stored after being encrypted using BCrypt hashing.
ii.	Welcome Email
After successful registration, the system sends a welcome email to the user’s provided address.
This email (supposedly) contains a link that redirects the user to the Create Employee Page, where they can complete their profile.

iii.	Create Employee Record
On this page, the user fills out their employee details — such as the full name, gender, department, and other relevant credentials.
Once submitted, the data is validated and stored in the SQL database using Entity Framework Core (EF Core).
The employee record is linked to the UserId from the signup.
The role defaults to "Pending" at creation.

iv.	Login and Token Generation
When the user attempts to log in, the system validates their credentials.
Upon successful verification:
•	A JWT token is generated.
•	The token includes claims that contain key user details (such as employee ID, role, and permissions).
•	This token acts as the identity pass that grants access to authorized areas of the system.
v.	Role-Based Access Control (RBAC)
The system implements role-based access control to determine what resources a user can access. Every protected endpoint is decorated with [HasPermission("...")]
•	If the user has created an employee record, their token will include permissions based on their assigned role (e.g., Admin, Manager).
•	If the user has not created an employee record, their token will still be issued but without any role-based permissions, meaning they will not have access to employee-specific resources.
vi.	Permission Verification (Helper Class)
Before accessing any protected endpoint, a Permission Helper Class and middleware check the user’s token and claims to confirm whether the user is authorized to perform that action.
•	If the permission check passes → the request proceeds to the controller.
•	If it fails → the system returns an appropriate response (e.g., 403 Forbidden).
vii.	Secure Session Management
Since the system uses JWT, it is stateless — meaning the server does not store any session data.
All necessary user information is embedded within the token, improving scalability and performance.

viii.	Upon creation of employee record, the employee role is initially stored as pending. In this state, the employee has access to basic employee resources like the GetEmployeebyId endpoint, GetProductsById, e.tc. 

Then Admin who has the permission to assigns roles with permission, checks for all pending employees and then, assigns them to their various roles. Once updated, the employee’s token (on next login) will include new permissions.



