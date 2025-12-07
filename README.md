# Gym Management System

A robust and modular **Gym Management System** built with **ASP.NET Core MVC**, following clean architecture principles and featuring a full authentication system, session scheduling, membership management, booking, cancellation, and Unit of Work pattern.

## 🚀 Technologies Used

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **Unit of Work + Generic Repository Pattern**
- **Automapper**
- **Microsoft Identity (Authentication & Authorization)**
- **SQL Server**
- **Bootstrap 5**
- **jQuery + Select2**

## 📌 Main Features

### 🔐 Authentication & Identity

- Login, Logout, Register using **ASP.NET Core Identity**
- Role-based access control (Admin – Trainer – Member)

### 🧑‍💼 Member Management

- Add / Edit / Delete members
- View active/inactive members
- Track membership status & subscription dates

### 📘 Membership System

- Create different membership packages (Monthly, Yearly…)
- Track member subscriptions & renewal

### 🗓️ Session Scheduling

- Create training sessions with:
  - Start Date
  - End Date
  - Coach
  - Capacity
- Prevent assigning a member to:
  - A past session
  - A full session
  - A session already booked

### 🧾 Session Booking

- Book members into upcoming sessions
- Prevent double booking
- Cancel booking
- Track booking date
- Track attendance (IsAttend true/false)

### 👥 Session Members View

- Display all members enrolled in a session
- Show booking date
- Allow cancelation

### 🎯 Unit of Work Pattern

Full implementation of **UnitOfWork + Repository Pattern**:

- Cleaner code
- Better transaction control
- Centralized saving

### 🔄 AutoMapper

Used to map:

- Entities → ViewModels
- ViewModels → Entities

### 🖥️ UI & Frontend

- Fully responsive Bootstrap UI
- Select2 dropdowns
- Validation messages
- Alert messages (success / error)

---

## 📁 Project Structure

```
GymManagementSystem/
│── BLL/                  # Business Logic Layer
│── DAL/                  # Data Access Layer
│── PresentationLayer/    # MVC UI Layer
│── wwwroot/              # Static files
│── Models/               # Database Entities
│── ViewModels/           # Data transfer to views
│── Services/             # Business services
│── Repositories/         # UnitOfWork + Generic Repository
```

---

## 🛠️ How to Run

1. Clone the repository
   ```bash
   git clone https://github.com/Mahmudul07-prog/Gym_Management_System.git
   ```
2. Update **appsettings.json** with your SQL Server connection string.
3. Run migrations:
   ```bash
   update-database
   ```
4. Run the project.

---

## 🤝 Contributing

Pull requests are welcome!

---

## 📜 License

This project is licensed under the **MIT License**.
