# Booking API

This project is a booking API built with ASP.NET Core. It provides endpoints for managing users and housings, and for booking housings.

## Endpoints

### POST /api/ApiController/createHousing

Creates a new housing.

**Request body:**

- `HousingInputModel` object

**Responses:**

- 200 OK: Returns a success message.
- 400 Bad Request: Returns an error message.

### GET /api/ApiController/getAllHousing

Gets all housings.

**Responses:**

- 200 OK: Returns a list of `Housing` objects.

### GET /api/ApiController/getAvailableHousing

Gets all available housings.

**Responses:**

- 200 OK: Returns a list of `Housing` objects.

### POST /api/ApiController/createUser

Creates a new user.

**Request body:**

- `UserInputModel` object

**Responses:**

- 200 OK: Returns a success message.
- 400 Bad Request: Returns an error message.

### GET /api/ApiController/getHousingByName

Gets a housing by name.

**Request parameters:**

- `name`: The name of the housing.

**Responses:**

- 200 OK: Returns a `Housing` object.

### PATCH /api/ApiController/removeBooking

Removes a booking.

**Request parameters:**

- `name`: The name of the housing.

**Responses:**

- 200 OK: Returns a success message.

### POST /api/ApiController/logIn

Logs in a user.

**Request parameters:**

- `logIn`: The login of the user.
- `password`: The password of the user.

**Responses:**

- 200 OK: Returns a success message.
- 400 Bad Request: Returns an error message.

### POST /api/ApiController/bookHousing

Books a housing.

**Request parameters:**

- `housingName`: The name of the housing.
- `userEmail`: The email of the user.
- `startDate`: The start date of the booking.
- `endDate`: The end date of the booking.

**Responses:**

- 200 OK: Returns a success message.
- 400 Bad Request: Returns an error message.

### PATCH /api/ApiController/changePassword

Changes a user's password.

**Request parameters:**

- `login`: The login of the user.
- `oldPassword`: The old password of the user.
- `newPassword`: The new password of the user.

**Responses:**

- 200 OK: Returns a success message.

## Models

The project uses several models to represent data:

- `User`: Represents a user with properties for login, email, password, first name, last name, and a list of reserved accommodations.
- `Housing`: Represents a housing with properties for ID, name, location, description, price per month, availability, booking start date, and booking end date.
- `UserDto`: Data transfer object for a user, used when returning user data to the client.
- `UserInputModel`: Represents the data needed to create a new user.
- `HousingInputModel`: Represents the data needed to create a new housing.

# PasswordHasher

The `PasswordHasher` class is part of the `Booking.Security` namespace. It provides functionality for hashing and verifying passwords using the PBKDF2 algorithm.

## Methods

### HashPassword(string password)

This method hashes a password using a new random salt and the PBKDF2 algorithm.

**Parameters:**

- `password`: The plain text password to be hashed.

**Returns:**

- A tuple containing:
  - `hashedPassword`: The hashed password as a base64-encoded string.
  - `salt`: The salt used for hashing as a base64-encoded string.

**Process:**

1. Generates a new random salt.
2. Creates a PBKDF2 instance with the password, salt, and 10000 iterations.
3. Generates a 20-byte hash from the PBKDF2 instance.
4. Combines the salt and hash into a single byte array.
5. Converts the combined byte array to a base64-encoded string.
6. Returns the hashed password and salt.

### VerifyPassword(string enteredPassword, string savedPasswordHash, string savedSalt)

This method verifies a password by comparing it to a saved hashed password.

**Parameters:**

- `enteredPassword`: The password to verify.
- `savedPasswordHash`: The saved hashed password.
- `savedSalt`: The salt used to hash the saved password.

**Returns:**

- `true` if the entered password matches the saved password, `false` otherwise.

**Process:**

1. Converts the saved salt and hashed password from base64-encoded strings to byte arrays.
2. Creates a PBKDF2 instance with the entered password, salt, and 10000 iterations.
3. Generates a 20-byte hash from the PBKDF2 instance.
4. Compares the generated hash to the hash part of the saved hashed password.
5. Returns `true` if all bytes match, `false` otherwise.

# Housing Management System

This system allows users to manage and book housings.

## Methods

### GetHousingData
This method retrieves all the housing data from the database.

### GetHousing
This method retrieves a specific housing by its name.

### RemoveBooking
This method removes a booking from a specific housing by its name.

### GetAvailableHousing
This method retrieves all available housings.

### BookHousing
This method books a specific housing for a user within a specified date range.

### ParseDate
This is a helper method that parses a date string into a `DateOnly` object.

### AddHousing
This method adds a new housing to the database.

## Usage

To use these methods, you need to provide the necessary parameters. For example, to book a housing, you need to provide the housing name, user email, and the start and end dates.

## Error Handling

Each method includes error handling to ensure that the system runs smoothly. For example, if a housing is not found or a booking date is invalid, the method will throw an exception.

## Future Improvements

Future improvements could include adding more features such as updating housing details, managing user profiles, and more.

# User Management System

This system allows users to create an account, log in, and change their password.

## Methods

### CreateUser
This method creates a new user in the database. It checks if the user already exists and hashes the password before storing it.

### LogIn
This method logs in a user. It checks if the user exists and verifies the password.

### ChangePassword
This method changes a user's password. It verifies the old password before hashing and storing the new password.

## Usage

To use these methods, you need to provide the necessary parameters. For example, to create a user, you need to provide the first name, last name, email, login, and password.

## Error Handling

Each method includes error handling to ensure that the system runs smoothly. For example, if a user is not found or a password is invalid, the method will return an error message.

## Future Improvements

Future improvements could include adding more features such as updating user details, managing user profiles, and more.

