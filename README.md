# PreparationTask

**Features**

Service for creating streets by points, adding new points and deleting streets.
Adding a point is implemented algorithmically on the backend and using PostGIS.

**Optional:**
- adding a point is thread-safe
- the database supports migration (use EntityFramework tools)
- unit testing and error handling

**Technologies:**
- .NET 9
- PostgreSQL 17.2 (PostGIS  extention) and pgAdmin 4
- EntityFrameworkCore 9.0
- Npgsql.EntityFrameworkCore.PostgreSQL 9.0.1
- AutoMapper
- docker and docker-compose
- Kubernetes
- NUnit and NSubstitute

**Architecture**
- Controller
- Services
- Data Access Layer
- DataTransfer
- Dependency Injection
- DbContextFactory

- SOLID: Development was strictly based on SOLID principles.
- Error Handling and support friendly user messages to user
- Unit testing using NUnit and NSubstitute packages.

**1. [Recommended] Local check using docker compose.**

- In the *PreparationTask/PreparationTaskService* directory you can run the command:

    `docker-compose up --build`

- The build process will start and the containers will start automatically, in the command line you can watch the process

- After the containers are running you can test requests. Three endpoints available.

**http://localhost:8080/street/create**

Request:
```json
{
    "StreetCreateRequest": {
        "Name": "test street 2",
        "Points": [
            {
                "Longitude": 22.94,
                "Latitude": 40.64
            },
            {
                "Longitude": 22.95,
                "Latitude": 40.64
            }
        ],
        "Capacity": 5
    }
}
```

Expected response:
```json
{
    "streetResponse": {
        "state": 0,
        "message": "The street created"
    }
}
```

**http://localhost:8080/street/delete**

Request:
```json
{
    "StreetDeleteRequest": {
        "Name": "test street 2"
    }
}
```

Expected response:
```json
{
    "streetResponse": {
        "state": 0,
        "message": "The street removed"
    }
}
```

**http://localhost:8080/street/addpoint**

Request:
```json
{
    "StreetAddPointRequest": {
        "Name": "test street 2",
        "NewPoint": {
            "Longitude": 22.96,
            "Latitude": 40.65
        }
    }
}
```

Expected response:
```json
{
    "streetResponse": {
        "state": 0,
        "message": "The new point added"
    }
}
```

By default, Thread safe implementation of adding a point with an algorithm on the backend is used.

To switch to adding via SQL script, you need to change the setting in the appsettings.json file.

```json
  "Features": {
    "UsePostGisToAddPoint": true
  }
```

Error messages are also possible: "The street already existing", "The point already existing" etc.

If you need to remove the container from the previous testing use the command

   `docker-compose down --volumes`


**2. Local testing and debugging using IDE.**

- In the *PreparationTask\PreparationTaskService\database* directory you can run the command:

    `docker-compose up --build`

as a result, the postgres database will be built and launched

- in IDE chage connection string:

`"DefultConnection": "Server=localhost;Port=5432;Database=preparationtask;User Id=postgres;Password=postgres"`

- in the IDE package manager you need to run the migration script

    `Update-Database`

- for local testing use port: "http://localhost:44445"

**3. Using Kubernetes to create a Deployment**

- start your minikube

    `minikube start`

- in the *PreparationTask/PreparationTaskService* directory you can run the command:

    `docker build -f PreparationTaskService\Dockerfile -t <!username!>/preptask:latest .`

or make a pull of an already prepared image

   `docker pull sergbelom/preptask`

- in the *PreparationTask/PreparationTaskService* directory you can apply a configuration to a resource by file name 

    `kubectl apply -f kubernetes.yaml`

- then run the command and get the URL to connect to the service

    `minikube service preptask-service`
