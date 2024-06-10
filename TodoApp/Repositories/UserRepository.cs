using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TodoApp.Data;
using TodoApp.Model;

namespace TodoApp.Repositories
{
    // repository are for encapsulating the data access code
    public class UserRepository : IUserRepository
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;
        public UserRepository(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<User>> GetUsers() => await _context.Users.ToListAsync();
        public async Task<User> Register(AddUser adduser)
        {
            var getuser = await _context.Users.FirstOrDefaultAsync(p=>p.Username == adduser.Username);
            if (getuser != null)
            {
                throw new UserNameAlreadyExists("username already taken try a new one.");
            }
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(adduser.Password);
            var user = new User() { 
                Username = adduser.Username,
                Password = passwordhash,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<TokenResponse> Login(AddUser adduser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == adduser.Username);
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(adduser.Password, user.Password);
            if (!verified)
            {
                throw new InvalidPasswordException("invalid password");
            }
            string token = CreateToken(user);
            string refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            await _context.SaveChangesAsync();
            return new TokenResponse { 
                AccessToken = token,
                RefreshToken = refreshToken,
            };

        }
        private string CreateToken(User user)
        {
            // claims are pieces of information 
            List<Claim> claims = new List<Claim>
            {
                // claim representing username
                new Claim(ClaimTypes.Name,user.Username)
            };
            // symmetric key means same key is used for encryption and decryption
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            //This represents the cryptographic key and security algorithms
            //that will be used to create a digital signature for the JWT token.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            //This class is used to generate a string representation of the JWT token.
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string GenerateRefreshToken()
        {
            // creates an array of 32 bytes
            var randomNumber = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                // this fills sthe randomNumber array with cryptographically strong randombytes
                rng.GetBytes(randomNumber);
                // converts the byte array into a Base64 encoded string representation
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task<TokenResponse> RefreshAccessToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.RefreshToken == refreshToken);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }
            string accessToken = CreateToken(user);
            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
