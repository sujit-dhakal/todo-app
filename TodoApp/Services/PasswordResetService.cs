
using TodoApp.Repositories;

namespace TodoApp.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetRepository _context;

        public PasswordResetService(IPasswordResetRepository context)
        {
            _context = context;
        }
        public string GeneratePasswordResetToken() => _context.GeneratePasswordResetToken();

        public Task ResetPasswordAsync(string token, string newPassword) => _context.ResetPasswordAsync(token, newPassword);

        public Task SendPasswordResetEmail(string Email, string Token) => _context.SendPasswordResetEmail(Email, Token);

        public Task StorePasswordResetToken(string Email, string Token, DateTime ExpiryTime)
            => _context.StorePasswordResetToken(Email, Token, ExpiryTime);

        public Task<bool> ValidatePasswordResetToken(string token) => _context.ValidatePasswordResetToken(token);
    }
}
