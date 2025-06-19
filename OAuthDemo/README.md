# OAuth 2.0 Authentication Demo

OAuth authentication with Google, Microsoft, and Facebook providers using claims-based identity.

## Setup

1. **Configure OAuth Providers** in `appsettings.json`:
   - Google: [Google Cloud Console](https://console.cloud.google.com)
   - Microsoft: [Azure App Registrations](https://portal.azure.com)
   - Facebook: [Facebook for Developers](https://developers.facebook.com)

2. **Redirect URIs**:
   - Google: `https://localhost:7000/signin-google`
   - Microsoft: `https://localhost:7000/signin-microsoft`
   - Facebook: `https://localhost:7000/signin-facebook`

3. **Run**:
   ```bash
   cd D:\Dev\repos\AdvCS\OAuthDemo
   dotnet restore
   dotnet run
   ```

## OAuth Flow

1. User clicks "Continue with Google"
2. Redirected to provider's authorization server
3. User grants permissions
4. Provider returns authorization code
5. App exchanges code for access token
6. App retrieves user profile/claims
7. Claims stored in authentication cookie

## Key Features

- **Multiple Providers**: Google, Microsoft, Facebook
- **Claims Mapping**: Provider claims → application claims
- **Cookie Storage**: Secure HttpOnly cookies
- **Profile Pictures**: From provider APIs
- **Email Verification**: Provider verification status

## Claims Retrieved

- `ClaimTypes.NameIdentifier` - Unique user ID
- `ClaimTypes.Email` - Email address
- `ClaimTypes.Name` - Display name
- `ClaimTypes.GivenName` - First name
- `ClaimTypes.Surname` - Last name
- `picture` - Profile image URL
- `email_verified` - Email verification status

## vs Other Auth Methods

| Feature | OAuth | JWT | Identity |
|---------|-------|-----|----------|
| **User Store** | External | Custom/DB | Database |
| **Claims Source** | Provider | Custom | Database |
| **Setup** | Provider config | Token logic | EF migration |
| **State** | Stateful cookies | Stateless | Stateful |

OAuth delegates authentication to trusted providers, eliminating password management while providing rich user claims.
