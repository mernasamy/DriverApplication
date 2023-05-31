# Driver Web API Documentation

Driver is a web API application built using ASP.NET Core that provides access to data stored in a relational database. The application is designed to provide a simple interface for querying and modifying the data, and is intended for use by a variety of clients, including web applications, mobile apps, and other services.

## Architecture

The application architecture consists of the following components:

### SqlHelper

`SqlHelper` is a static class that contains a set of methods for interacting with the database using ADO.NET. These methods include:

- `ExecuteReader`: Executes a SQL command and returns a list of objects of type T that represent the results of the query.
- `ExecuteScalar`: Executes a SQL command and returns a single value.
- `ExecuteNonQuery`: Executes a SQL command that does not return any results.

Each method accepts a connection string, command text, and parameters as input, and uses ADO.NET to execute the command against the database.

### Repository

`Repository` is a class that wraps the `SqlHelper` methods and provides a higher-level interface for interacting with the database. The `Repository` class accepts an `IOptions<DbSettings>` object in its constructor to configure the database connection settings. Methods in the `Repository` class include:

- `GetAll`: Retrieves all drivers in a table.
- `GetById`: Retrieves a single driver by its ID.
- `Create`: Creates a new driver in the table.
- `Update`: Updates an existing driver.
- `Delete`: Deletes an driver from the table.

Each method calls the appropriate `SqlHelper` method to execute the corresponding SQL command against the database.

### Service

`Service` is a class that implements the business logic of the application. The `Service` class accepts an instance of `Repository` in its constructor and uses it to interact with the database. Methods in the `Service` class include:

- `GetAll`: Retrieves all drivers from the database.
- `GetById`: Retrieves a single driver by its ID.
- `Create`: Creates a new driver in the database.
- `Update`: Updates an existing driver.
- `Delete`: Deletes an driver from the database.

Each method calls the appropriate `Repository` method to perform the database operation, and may perform additional validation or business logic as needed.

### API Functions

The API functions are implemented as ASP.NET Core endpoints that expose the functionality of the `Service` class to clients. Each endpoint accepts input from the client as JSON data, passes it to the appropriate `Service` method to perform a database operation, and returns the results as JSON data. Endpoints include:

- `GET /Drivers`: Retrieves all drivers from the database.
- `GET /Drivers/{id}`: Retrieves a single driver by its ID.
- `POST /Drivers`: Creates a new driver in the database.
- `PUT /Drivers/{id}`: Updates an existing driver.
- `DELETE /Drivers/{id}`: Deletes an driver from the database.

Each endpoint may have additional query parameters or headers as needed.

### ErrorHandlerMiddleware

`ErrorHandlerMiddleware` is a middleware component that intercepts any uncaught exceptions that occur during request processing and returns an appropriate HTTP response with an error message. This middleware is responsible for handling errors gracefully and ensuring that clients receive informative error messages when something goes wrong.

## Endpoint Documentation

### GET /Drivers

Retrieves all drivers from the database.

Parameters:

- None.

Example Request:

```http
GET /Drivers HTTP/1.1
Host: example.com
```

Example Response:

```json
[
    {
        "id": 1,
        "firstName": "Driver1",
        "lastName": "Driver1",
        "email": "driver1@example.com",
        "phoneNumber": "123456789"
    },
    {
       "id": 1,
        "firstName": "Driver1",
        "lastName": "Driver1",
        "email": "driver1@example.com",
        "phoneNumber": "987654321"
    }
]
```

### GET /Drivers/{id}

Retrieves a single driver by its ID.

Parameters:

- `id`: The ID of the driver to retrieve.

Example Request:

```http
GET /Drivers/1 HTTP/1.1
Host: example.com
```

Example Response:

```json
{
     "id": 1,
     "firstName": "Driver1",
     "lastName": "Driver1",
     "email": "driver1@example.com",
     "phoneNumber": "987654321"
}
```

### POST /Drivers

Creates a new driver in the database.

Parameters:

- `firstName` (required): The first name of the driver.
- `lastName`: (required): The last name of the new driver.
- `email`: (required): The email of the new driver.
- `phoneNumber`: (required): The phone number of the new driver.

Example Request:

```http
POST /Drivers HTTP/1.1
Host: example.com
Content-Type: application/json

{
     "firstName": "new driver",
     "lastName": "new driver",
     "email": "newdriver@example.com",
     "phoneNumber": "987654321"
}
```

Example Response:

```json
{
    "id": 3,
     "isSuccess": true,
     "message": "Driver created"
}
```

### PUT /Drivers/{id}

Updates an existing driver.

Parameters:

- `id`: The ID of the driver to update.
- `firstName`: The first name of the driver to update.
- `lastName`: The last name of the driver to update.
- `email`: The email of the driver to update.
- `phoneNumber`: The phone number of the driver to update.

Example Request:

```http
PUT /Drivers/1 HTTP/1.1
Host: example.com
Content-Type: application/json

{
     "firstName": "update driver",
     "lastName": "update driver",
     "email": "updatedriver@example.com",
     "phoneNumber": "987654321"
}
```

Example Response:

```json
{
     "isSuccess": true,
     "message": "Driver updated"
}
```

### DELETE /Drivers/{id}

Deletes an driver from the database.

Parameters:

- `id`: The ID of the driver to delete.

Example Request:

```http
DELETE /Drivers/1 HTTP/1.1
Host: example.com
```

Example Response:
```json
{
     "isSuccess": true,
     "message": "Driver deleted"
}
```

```http
HTTP/1.1 204 No Content
```

## Error Handling

The `ErrorHandlerMiddleware` is responsible for handling errors that occur during request processing and returning an appropriate HTTP response. The middleware can handle the following types of errors:

- `NotFoundException`: Raised when a requested resource does not exist.
- `BadRequestException`: Raised when a request contains invalid data.
- `InternalServerErrorException`: Raised when an unexpected error occurs.

If an error occurs, the middleware returns a JSON response with an appropriate HTTP status code and error message. For example:

```json
{
    "error": "Bad Request",
    "message": "The request data is invalid"
}
```

## Dependencies

The application depends on the following third-party libraries:

- `Microsoft.AspNetCore.Mvc`: Provides the infrastructure for building web APIs.
- `Microsoft.Extensions.Configuration`: Provides configuration support for the application.
- `Microsoft.Extensions.Options`: Provides an easy way to configure options for the application.
- `System.Data.SqlClient`: Provides ADO.NET functionality for interacting with SQL Server databases.
