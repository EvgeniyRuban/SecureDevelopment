namespace CardStorageService.Domain;

public enum AuthenticationStatus
{
    Success,
    AccountLocked,
    UserNotFound,
    InvalidPassword
}