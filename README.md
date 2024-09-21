# dotNet5784_5970_5654
# Project Management System

MiniProject5784  
Developed by Kaduri and Imanuel Perel

## Overview

This is a multi-tier project management system developed in C# using the .NET Framework. The system allows project managers to efficiently manage tasks, milestones, and team members, while engineers can update and view their assigned tasks. The application uses WPF for the client-side interface and stores data in XML files. It is designed to streamline project timelines and improve collaboration within teams.

## Features

### For Project Managers:
- **Task Management**: Create, update, and assign tasks to engineers.  
  The admin interface allows viewing and managing tasks efficiently.  
  ![Task view](https://github.com/user-attachments/assets/64893796-5d41-49bc-b44a-eada9e2809d8)

- **Engineer Management**: Add and manage engineers, view tasks assigned to engineers.  
  The admin can view a list of all engineers and their task assignments.  
  ![Engineer View](https://github.com/user-attachments/assets/64893796-5d41-49bc-b44a-eada9e2809d8)

- **Milestone Tracking**: Automatically calculate and manage project milestones based on task dependencies.

- **Progress Monitoring**: Monitor project progress and task completion through a user-friendly dashboard.

### For Engineers:
- **Task Updates**: View assigned tasks, update progress, and mark tasks as complete.
- **Task Selection**: View available tasks and select new tasks upon completion of existing ones.

## Architecture

The system is structured into three primary layers:
- **Data Access Layer (DAL)**: Handles CRUD operations and stores data in XML files.
- **Business Logic Layer (BLL)**: Implements core functionalities, such as task and milestone management.
- **Presentation Layer**: WPF-based interface for interacting with the system.

## Technologies
- **Framework**: .NET (7.0+)
- **Language**: C#
- **UI**: WPF (Windows Presentation Foundation)
- **Data Storage**: XML
- **Development Environment**: Visual Studio (2022+)

## Installation

### Prerequisites:
- .NET Framework 7.0 or higher
- Visual Studio 2022 or higher

### Steps:
1. Clone the repository:
    ```
    git clone <repository_url>
    ```
2. Open the solution file in Visual Studio.
3. Restore NuGet packages.
4. Build the solution.

## Usage

### Project Manager:
1. Create a new project, defining start and end dates.
2. Add engineers to the project and assign tasks.  
   The admin can add new engineers through an intuitive interface.  
   ![Add Engineer](https://github.com/user-attachments/assets/5fe7360d-62d8-4cb1-9347-76a888d392c1)

3. Use the milestone calculation tool to generate project milestones.
4. Monitor and update project progress as needed.

### Engineer:
1. Log in to view current tasks.
2. Update task progress and mark tasks as complete.
3. Select new tasks when available.

## Data Structure

### Entities:
- **Task**: Contains information about task title, description, assigned engineer, dependencies, and progress status.
- **Engineer**: Stores information about the engineers, including name, skill level, and task assignments.
- **Milestone**: Automatically calculated based on task dependencies and project progress.

### XML Storage:
- Data is saved in XML format, including tasks, engineers, and project configuration.

## Bonuses

### 1. Clock Functionality
- **Saved and Restored:** The state of the clock is saved and restored when the application is reopened.  
  *Implementation: `BO.Tools` (lines 373-417), `MainWindow.xaml.xaml.cs` (lines 44-61)*

### 2. Auto Scheduling
- Automatically schedules tasks based on predefined rules and conditions.  
  *Implementation: `Bl.taskImplementation` (lines 368-492)*

### 3. Custom UI Enhancements
- **Shape Customization:** Unique shapes used in the user interface.  
  *Implementation: `MainWindow.xaml` (lines 30-40)*
- **Icons:** Custom icons for visual enhancement.  
  *Implementation: `MainWindow.xaml` (line 10), `TaskListWindow.xaml` (line 9)*

### 4. Gantt Chart
- **Dynamic Colors:** The Gantt chart changes its colors dynamically to reflect different project statuses.  
  ![Gantt Chart](https://github.com/user-attachments/assets/51c5755b-1abe-440d-a936-14c30ca371d3)
  *Implementation: `GaunttWindow1.xaml.cs` (lines 82-86)*
- **Animation Trigger:** Gantt chart animations are triggered by specific events.  
  *Implementation: `ManagerInterfaceMainWindow.xaml` (lines 41-79)*

### 5. Interactive Buttons
- **Hover Color Change:** Buttons change color when hovered over with the mouse.  
  *Implementation: `App.xaml` (lines 66-68)*

### 6. Data Validation
- **Content Trigger:** If the entered ID is too short, the field block turns red to alert the user.  
  *Implementation: `EngineerWindow.xaml` (line 39)*

### 7. Parsing and Testing
- **TryParse Implementation:** Includes `TryParse` for robust error handling during tests in DAL and BLL.  
  *Implementation: `DalTest.Program` (lines 475-482), `BlTest.Program` (lines 320-335)*

### 8. Additional Features
- **ToString Override:** Custom `ToString()` method for formatting object data.  
  *Implementation: `BO.Tools` (lines 31-120)*
- **Clock Animation:** The clock ticks every second for a real-time display.  
  *Implementation: `MainWindow.xaml.cs` (lines 38-42, 72-91)*
