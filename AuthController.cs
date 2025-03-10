using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace LoanManagementSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ User Registration
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null) return BadRequest("Email already registered.");

            // ✅ Encrypt Password
            user.PasswordHash = HashPassword(user.PasswordHash);

            await _userRepository.AddUser(user);
            await _userRepository.SaveChanges();

            return Ok(new { message = "Registration successful" });
        }

        // ✅ User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmail(loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password.");

            return Ok(new { userId = user.Id, message = "Login successful" });
        }

        // ✅ Hash Password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // ✅ Verify Password
        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return enteredHash == storedHash;
        }
    }

    // ✅ Login Request DTO
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
