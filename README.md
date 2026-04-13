# 🧠 Course Management System (CMS) – Minimal API

A modern backend system built with ASP.NET Core Minimal APIs for managing courses, students, and enrollments.

This project focuses on clean architecture principles, structured logging, and production-ready API design using modern .NET practices.

---

## 📌 Overview

The Course Management System (CMS) is a backend web API that allows:

- Managing courses
- Managing students
- Enrolling students into courses

The system is designed with scalability and maintainability in mind, using a layered architecture and clean separation of concerns.

---

## 🚀 Tech Stack

- C#
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQL Server
- AutoMapper
- Serilog
- FluentValidation

---

## 🏗️ Architecture

The project follows a layered architecture:


Presentation (Endpoints)
Application (Services, DTOs, Interfaces)
Domain (Entities)
Infrastructure (DbContext, EF Core)
Common (Result Pattern, Errors)



### 🔹 Presentation Layer
- Minimal API endpoints
- Route grouping (`/courses`, `/students`, `/enrollments`)
- Endpoint filters for validation

### 🔹 Application Layer
- Business logic inside services
- DTOs for request/response handling
- Interfaces for abstraction

### 🔹 Domain Layer
- Core entities:
  - Course
  - Student
  - Enrollment

### 🔹 Infrastructure Layer
- Entity Framework Core (DbContext)
- Database configuration

---

## 🗄️ Database Design

### Entities

#### 📘 Course
- Id
- Title
- Description
- Price
- CourseType
- CreatedAt

#### 👤 Student
- Id
- Name
- Email

#### 🔗 Enrollment
- Id
- CourseId (FK)
- StudentId (FK)
- EnrolledAt

---

### Relationships

- One Course → Many Enrollments  
- One Student → Many Enrollments  
- Many-to-Many between Course and Student via Enrollment  

---

## ⚙️ Features

### ✅ Course Management
- Create Course
- Get All Courses
- Get Course by ID
- Update Course
- Delete Course

---

### ✅ Student Management
- Add Student
- Get Students
- Update Student
- Delete Student

---

### ✅ Enrollment System
- Enroll a student in a course
- Prevent duplicate enrollments
- Track enrollment history

---

### ✅ Validation Pipeline
- Implemented using FluentValidation
- Custom `ValidationFilter` applied to endpoints
- Ensures clean separation between validation and business logic

---

### ✅ Result Pattern (Centralized Response Handling)
- Unified success/failure responses
- Avoids exception-driven flow
- Consistent API behavior

---

### ✅ Global Exception Handling
- Middleware to catch unhandled exceptions
- Prevents API crashes
- Returns standardized error responses

---

### ✅ Structured Logging
- Implemented using Serilog
- Logs important application events and errors
- Supports structured, contextual logging

---

### ✅ DTO Mapping
- AutoMapper used to map between entities and DTOs
- Prevents exposing domain models directly

---

## 🧠 Architectural Decisions

### 🔹 Why no Repository Pattern?

This project uses Entity Framework Core's `DbContext` directly instead of implementing a custom Repository layer.

Entity Framework Core already provides built-in implementations of:
- Repository pattern (`DbSet<T>`)
- Unit of Work pattern (`DbContext`)

Adding an additional repository abstraction would introduce unnecessary complexity without significant benefit for this project.

### 🔹 Focus Areas Instead

The project focuses on:

- Clean API design using Minimal APIs  
- Structured logging using Serilog  
- Consistent error handling using Result pattern  
- Validation using FluentValidation and endpoint filters  

This keeps the codebase simple, maintainable, and aligned with modern .NET practices.

---

## 📡 API Endpoints

### 📘 Courses


GET /courses

GET /courses/{id}

POST /courses

PUT /courses/{id}

DELETE /courses/{id}

---

### 👤 Students


GET /students

GET /students/{id}

POST /students

PUT /students/{id}

DELETE /students/{id}

---

### 🔗 Enrollments


POST /enrollments

GET /enrollments

GET /enrollments/{id}

---

## 🧪 Example Request

### Create Course


POST /courses

{
  "title": "ASP.NET Core",
  "description": "Learn backend development",
  "price": 100,
  "courseType": "Online"
}

---

## 💼 What This Project Demonstrates

* Strong understanding of backend architecture
* Practical use of Minimal APIs
* Clean separation of concerns
* Real-world API design practices
* Logging, validation, and error handling

---

## Summary

> I built a Course Management System using ASP.NET Core Minimal APIs with a layered architecture. I implemented structured logging with Serilog, validation using FluentValidation with a custom filter, and centralized error handling using a Result pattern and middleware. I chose to use EF Core directly instead of adding a repository layer to keep the architecture simple and aligned with modern practices.

---

## 📬 Feedback

Feel free to open an issue or reach out with feedback or suggestions.

