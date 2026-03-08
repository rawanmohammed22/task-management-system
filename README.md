# Task Management System (Clean Architecture)

A full-stack project and task management system built using ASP.NET Core MVC, heavily focused on architectural purity through **Clean Architecture** principles.

## Features
- **Project & Task Management:** Create, assign, and track projects and their sub-tasks.
- - **Collaboration:** Users can add comments on tasks.
  - - **Notifications:** Notification system to alert users of task updates.
    - - **Clean Architecture:** Strict separation between Core, Application, Infrastructure, and Presentation layers to ensure high maintainability.
     
      - ## Tech Stack
      - - **Backend Framework:** ASP.NET Core MVC (C#)
        - - **Architecture:** Clean Architecture
          - - **Database:** Entity Framework Core / SQL Server
           
            - ## Current Status & Known Limitations
            - *(Honest Assessment)*
            - - **UI UX Polish:** The frontend views are functional but lack modern styling and responsive design polish.
              - - **Tests:** Missing unit tests for the domain and application layers, which is crucial for proving the value of Clean Architecture.
                - - **Incomplete Features:** Some notification edge cases and advanced task filtering are not fully implemented.
                 
                  - ## How to Run
                  - 1. Open the solution `.sln` file in Visual Studio.
                    2. 2. Update the connection string in `appsettings.json`.
                       3. 3. In the Package Manager Console, run:
                          4.    ```powershell
                                   Update-Database
                                   ```
                                4. Run the project (F5).
                                5. 
