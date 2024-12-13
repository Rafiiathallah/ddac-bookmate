# Bookmate ☕

A cloud-based digital library application where book lovers can discover, read, and download their next favorite ebook. Built with modern web technologies and a passion for reading (and coffee!).

## Important Note

This is the main branch (currently empty). Frontend implementation using Next.js is being tested in `feature/sean-nextjs` branch.

## 📚 About

Bookmate is your personal digital library companion. Whether you're a night owl reading with a cup of coffee or an early bird starting your day with a book, we've got you covered.

### Key Features

- Register and login
- View books (searched by search bar or by genre)
- View authors and publishers and their related books
- Manage user account
- Manage library containing bought and borrowed books
- Modern UI/UX Experience

## Tech Stack

- **Frontend:** TESTING
- **Backend:** C# ASP.NET
- **Cloud Platform:** AWS

## Database Setup Guide 🗄️

### 1. Create Database in AWS RDS

- Create a new PostgreSQL database instance in AWS RDS
- Note down the connection details (endpoint, port, database name)
- Ensure proper security group settings are configured

### 2. Setup Database Connection

Configure your database connection string in the application:

- Update the host, username, and password according to your RDS configuration
- Ensure the connection string format is correct for PostgreSQL
- Test the connection before proceeding

### 3. Database Migration

The migration file `20241213113923_CreateTables` in the `Migration` folder contains:

- Table creation scripts
- Seed data for initial application content
- Relationship configurations

### 4. Update Database

To apply migrations and seed data:

1. Open Package Manager Console in Visual Studio
2. Run the command: `Update-Database`
3. This will:
   - Create all necessary database tables
   - Insert seed data (books, authors, genres, etc.)
   - Configure relationships between tables

### Default Login Credentials

After seeding the database, you can use these credentials:

- Admin:
  - Email: admin@example.com
  - Password: Password12345!
- User:
  - Email: user@example.com
  - Password: Password12345!
