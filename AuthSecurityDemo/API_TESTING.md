# API Testing Guide

This file contains examples for testing the Auth Security Demo APIs using curl, Postman, or other HTTP clients.

## Base URL
```
https://localhost:5001
```

## 1. Register a New User

### Request
```http
POST /api/api/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe",
  "age": 25
}
```

### curl Example
```bash
curl -X POST https://localhost:5001/api/api/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123",
    "firstName": "John",
    "lastName": "Doe",
    "age": 25
  }'
```

### Response
```json
{
  "success": true,
  "message": "Registration successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "email": "test@example.com",
      "firstName": "John"
    }
  }
}
```

## 2. Login

### Request
```http
POST /api/api/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "password123"
}
```

### curl Example
```bash
curl -X POST https://localhost:5001/api/api/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "password123"
  }'
```

### Response
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwibmFtZSI6IkpvaG4gRG9lIiwiYWdlIjoiMjUiLCJyb2xlIjoiVXNlciIsImV4cCI6MTY0MDk5NTIwMH0.signature",
    "user": {
      "id": 1,
      "email": "test@example.com",
      "firstName": "John"
    }
  }
}
```

## 3. Get User Profile (Requires Authentication)

### Request
```http
GET /api/api/profile
Authorization: Bearer YOUR_JWT_TOKEN
```

### curl Example
```bash
curl -X GET https://localhost:5001/api/api/profile \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Response
```json
{
  "success": true,
  "message": "Profile retrieved successfully",
  "data": {
    "userId": "1",
    "email": "test@example.com",
    "name": "John Doe",
    "age": "25",
    "roles": ["User"],
    "claims": [
      {"type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "value": "1"},
      {"type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "value": "test@example.com"},
      {"type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "value": "John Doe"},
      {"type": "age", "value": "25"},
      {"type": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "value": "User"}
    ]
  }
}
```

## 4. Get Claims Demo (Requires Authentication)

### Request
```http
GET /api/api/claims-demo
Authorization: Bearer YOUR_JWT_TOKEN
```

### curl Example
```bash
curl -X GET https://localhost:5001/api/api/claims-demo \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Response
```json
{
  "success": true,
  "message": "Claims information retrieved",
  "data": {
    "allClaims": [
      {"type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "value": "1"},
      {"type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "value": "test@example.com"},
      {"type": "age", "value": "25"}
    ],
    "specificClaims": {
      "userId": "1",
      "email": "test@example.com",
      "name": "John Doe",
      "givenName": "John",
      "surname": "Doe",
      "age": "25",
      "roles": ["User"]
    },
    "authenticationInfo": {
      "isAuthenticated": true,
      "authenticationType": "Bearer",
      "name": "John Doe"
    }
  }
}
```

## 5. Get Comments (Requires Authentication)

### Request
```http
GET /api/api/comments
Authorization: Bearer YOUR_JWT_TOKEN
```

### curl Example
```bash
curl -X GET https://localhost:5001/api/api/comments \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## 6. Get All Users (Admin Only)

### Request
```http
GET /api/api/users
Authorization: Bearer YOUR_ADMIN_JWT_TOKEN
```

### curl Example
```bash
curl -X GET https://localhost:5001/api/api/users \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## 7. Get Age Restricted Content (18+ Only)

### Request
```http
GET /api/api/restricted
Authorization: Bearer YOUR_JWT_TOKEN_WITH_AGE_18_PLUS
```

### curl Example
```bash
curl -X GET https://localhost:5001/api/api/restricted \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## Testing Different Authorization Scenarios

### 1. Test Without Token (Should get 401 Unauthorized)
```bash
curl -X GET https://localhost:5001/api/api/profile
```

### 2. Test With Invalid Token (Should get 401 Unauthorized)
```bash
curl -X GET https://localhost:5001/api/api/profile \
  -H "Authorization: Bearer invalid_token"
```

### 3. Test Admin Endpoint Without Admin Role (Should get 403 Forbidden)
```bash
# Login as regular user, then try admin endpoint
curl -X GET https://localhost:5001/api/api/users \
  -H "Authorization: Bearer REGULAR_USER_TOKEN"
```

### 4. Test Age Restriction With Under-18 User (Should get 403 Forbidden)
```bash
# Register user with age < 18, then try restricted endpoint
curl -X GET https://localhost:5001/api/api/restricted \
  -H "Authorization: Bearer UNDER_18_USER_TOKEN"
```

## Creating Admin Users

To test admin functionality, you'll need to manually create an admin user in the database:

```sql
-- 1. Find the user ID
SELECT Id FROM Users WHERE Email = 'your-email@example.com';

-- 2. Find the Admin role ID
SELECT Id FROM Roles WHERE Name = 'Admin';

-- 3. Assign admin role to user
INSERT INTO UserRoles (UserId, RoleId) VALUES (USER_ID, ADMIN_ROLE_ID);
```

Then login again to get a new JWT token with the Admin role.

## Postman Collection

You can import these requests into Postman by creating a new collection and adding each request. Set up environment variables:

- `base_url`: https://localhost:5001
- `jwt_token`: (set this after login)

Then use `{{base_url}}` and `{{jwt_token}}` in your requests.

## Common HTTP Status Codes

- `200 OK` - Request successful
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Endpoint not found
- `422 Unprocessable Entity` - Validation failed
