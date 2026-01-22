namespace CRMSystem.Application.Auth;

public static class AuthErrorCodes
{
    public const string InvalidId = "auth.invalid_id";
    public const string InvalidEmail = "auth.invalid_email";
    public const string InvalidPassword = "auth.invalid_password";
    public const string UserLocked = "auth.user_locked";
    public const string UserInactive = "auth.user_inactive";
    public const string JwtTokenGenerationFailed = "auth.token_generation_failed";
    public const string EmailAlreadyExists = "auth.email_already_exists";
    public const string EmailNotConfirmed = "auth.email_not_confirmed";
    public const string RoleNotFound = "auth.role_not_found";
    public const string UserCreationFailed = "auth.user_creation_failed";
    public const string AssigningRoleFailed = "auth.assigning_role_failed";
}