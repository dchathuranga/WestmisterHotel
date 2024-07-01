# Westminster Hotel Reservation System

Welcome to the Westminster Hotel Reservation System, a C# console application designed to manage room bookings for a hotel. This system allows customers to book rooms and check availability, while administrators can manage rooms and generate reports.

## Features

### Customer Features:
1. Book a Room
2. List Available Rooms

### Admin Features:
1. Add Room
2. Delete Room
3. List Rooms
4. List Rooms Ordered by Price
5. Generate Report

## Installation and Setup

### Prerequisites
- .NET 5.0 or later

### Steps to Run the Application in VS Code
1. **Clone the repository**:
    `git clone <repository-url>`
2. **Navigate to the project directory**:
    `cd WestminsterHotel`
3. **Open the project in Visual Studio Code**
4. **Install the C# extension for Visual Studio Code**:
    - Search for "C#" and install the extension provided by Microsoft.

5. **Restore dependencies**:
    `dotnet restore`
6. **Build the application**:
    `dotnet build`
7. **Run the application**:
    `dotnet run`

## Usage

### Main Menu
When you run the application, you will be greeted with the main menu:
```
Welcome to the Westminster Hotel Reservation System!

Please select an option:
1. Book a room
2. List available rooms
3. Navigate to admin menu
4. Exit program
```
Select an option by entering the corresponding number.

### Customer Booking
- **Book a Room**: Follow the prompts to enter the check-in and check-out dates, room number, and personal information.
- **List Available Rooms**: Enter the desired room size and optional maximum price to view available rooms.

### Admin Menu
Navigate to the admin menu by selecting option 3 in the main menu. You will be prompted to enter the admin credentials:
- **Username**: `admin`
- **Password**: `12345`

Once logged in, you can manage rooms and generate reports.

### Room Management
- **Add Room**: Enter the room details as prompted.
- **Delete Room**: Enter the room number to delete it from the system.
- **List Rooms**: View all available rooms.
- **List Rooms Ordered by Price**: View rooms ordered by price.
- **Generate Report**: Generate a report and save it to `report.txt`.