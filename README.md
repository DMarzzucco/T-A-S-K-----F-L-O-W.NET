# T A S K / / F L O W

TaskFlow is a versatile template for project and task management, designed to optimize collaboration and access control among users in any organizational environment. Its modular architecture allows the application to be customized and adapted to various needs, from corporate systems to educational platforms or personal productivity tools.

### With TaskFlow, administrators can:

- Register users securely and assign customized roles.
- Manage access levels to ensure that only authorized individuals can make critical modifications to projects and tasks.

### The application enables users to:

- Create, update, and delete projects and tasks.
- Monitor task progress with configurable statuses such as created, in progress, and finished.
- Collaborate efficiently, ensuring that each role has the tools necessary to fulfill its responsibilities.
Key features include:

- Role-based access control: Assigns flexible permission levels, such as administrator, creator, and specific roles like owner, maintainer, or developer.
- Secure authentication: Protects the system with robust validation and authorization mechanisms.
- Modular management: Facilitates customization to suit various industries, such as education, marketing, software development, or internal administration.

Thanks to its focus on security, efficiency, and adaptability, TaskFlow is the ideal solution for implementing collaborative systems that require structured control of users and activities.

### Possible Applications of TaskFlow
TaskFlow is a flexible solution that can be integrated into a wide variety of systems, including:

1. **Project Management**  
   Ideal for coordinating teams and tasks in corporate, educational, or creative environments, ensuring efficient organization and clear progress tracking.  

2. **Internal Collaboration Systems**
Simplifies the organization and monitoring of activities among users with diverse roles, fostering structured cooperation within teams.

3. **Educational Platforms**
Enables the management of courses, assignment tracking, and effective monitoring of student progress.

4. **Personal Productivity Tools**
Offers an efficient way to organize individual tasks and projects, catering to the needs of independent users.

> [!IMPORTANT]
> This project is still in development

# Data Model Structure

The system's data model consists of four interconnected entities. These entities are linked through foreign keys, defining their relationships and ensuring referential integrity within the database.

![Models](/img/ArchDB.jpg)

# Application Workflow

This diagram illustrates the application's logic flow, including user authentication, role-based access control, and task management processes.

![Models](/img/AppArch.jpg)


## Requirements

- [Docker-Desktope](https://www.docker.com/products/docker-desktop/)
- [.NET 8](https://dotnet.microsoft.com/es-es/download)

## Install

```bash

# start the date base
$ docker-compose up db

# in .\TASK-FLOW-TESTING\

# start the entities in date base
$ dotnet ef database update

# start the server
$ dotnet run

```

## Documentation Swagger

[PORT:5024](http://localhost:5024/swagger/index.html)

## Author 

Dario Marzzucco 
