namespace TodoApp.Services
{
    public interface IPasswordResetService
    {
        string GeneratePasswordResetToken();
        Task StorePasswordResetToken(string Email, string Token, DateTime ExpiryTime);
        Task SendPasswordResetEmail(string Email, string Token);
        Task<bool> ValidatePasswordResetToken(string token);
        Task ResetPasswordAsync(string token, string newPassword);
    }
}
