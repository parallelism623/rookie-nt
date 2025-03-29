# Project API Documentation

## Overview
This project provides a simple API for user authentication.

## API Endpoints

### Login
- **URL**: `https://localhost:7052/api/login?(locale=@)`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "username": "admin",
    "password": "admin123*A"
  }
  ```
- **Description**: Authenticates a user with the provided credentials.

## Notes
- Replace `[port]` with the actual port number your application is running on (e.g., `7052`).
- Ensure the API is running locally before making requests.
- Replace `@` in locale to choose language for request