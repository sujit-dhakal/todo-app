using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using TodoApp.Data;
using TodoApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoApp.Repositories
{
    public class PasswordResetRepository:IPasswordResetRepository
    {
        private readonly TodoContext _context;
        public PasswordResetRepository(TodoContext context)
        {
            _context = context;
        }
        public string GeneratePasswordResetToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task StorePasswordResetToken(string email, string token, DateTime expiryTime)
        {
            var passwordResetToken = new PasswordResetModel
            {
                Email = email,
                Token = token,
                ExpiryTime = expiryTime
            };
            await _context.PasswordResetModels.AddAsync(passwordResetToken);
            await SaveChanges();
        }

        public async Task SendPasswordResetEmail(string Email, string Token)
        {
            var userEmail = await _context.Users.FirstOrDefaultAsync(t  => t.Email == Email);
            if(userEmail == null)
            {
                throw new Exception("user not found");
            }
            var resetLink = $"todoapp/token={Token}";
            // for constructing and manipulating email messages
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Todo", "sujitramdhakal59@gmail.com"));
            message.To.Add(new MailboxAddress("", Email));
            message.Subject = "Password Reset";
            message.Body = new TextPart("plain") {
                Text = $"Click the link to reset your password {resetLink}"
            };
            using ( var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("sujitramdhakal59@gmail.com", "xftwxdexjhvamuwx");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<bool> ValidatePasswordResetToken(string token)
        {
            var passwordResetToken = await _context.PasswordResetModels.FirstOrDefaultAsync(t => t.Token == token);
            if (passwordResetToken == null || passwordResetToken.ExpiryTime < DateTime.UtcNow) {
                return false;
            }
            return true;
        }


        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var passwordResetToken = await _context.PasswordResetModels.FirstOrDefaultAsync(t => t.Token == token);
            if (passwordResetToken == null || passwordResetToken.ExpiryTime < DateTime.UtcNow)
            {
                throw new Exception("Invalid or token expired");
            }
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Email == passwordResetToken.Email);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.PasswordResetModels.Remove(passwordResetToken);
            await SaveChanges();    
        }
        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}
