using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManagementSystem.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ✅ Register a new user with password hashing
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Password is required.");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt)
            };

            await _userRepository.AddUser(newUser);
            await _userRepository.SaveChanges();

            return Ok("User registered successfully.");
        }

        // ✅ Get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var userDtos = users.Select(u => MapToUserDto(u)).ToList();
            return Ok(userDtos);
        }

        // ✅ Get a user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");
            var userDto = MapToUserDto(user);
            return Ok(userDto);
        }

        // ✅ Update user details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updatedUser)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            user.Username = updatedUser.Username ?? user.Username;
            user.Email = updatedUser.Email ?? user.Email;

            await _userRepository.SaveChanges();
            return Ok("User updated successfully.");
        }

        // ✅ Delete a user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            _userRepository.DeleteUser(user);
            await _userRepository.SaveChanges();

            return Ok("User deleted successfully.");
        }

        // ✅ Password Hashing Logic
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // ✅ Mapping Method
        private UserResponseDto MapToUserDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Loans = user.Loans?.Select(l => new LoanDto
                {
                    Id = l.Id,
                    Amount = l.Amount,
                    Purpose = l.Purpose,
                    MonthlySalary = l.MonthlySalary,
                    Status = l.Status,
                    AppliedTime = l.AppliedTime,
                    ApprovedTime = l.ApprovedTime,
                    RejectedTime = l.RejectedTime,
                    CreditCard = l.CreditCard != null ? new CreditCardDto
                    {
                        Id = l.CreditCard.Id,
                        CardNumber = l.CreditCard.CardNumber,
                        ExpiryDate = l.CreditCard.ExpiryDate,
                        CVV = l.CreditCard.CVV,
                        ApprovedAmount = l.CreditCard.ApprovedAmount,
                        LoanId = l.CreditCard.LoanId
                    } : null
                }).ToList() ?? new List<LoanDto>(),
                CreditCards = user.CreditCards?.Select(c => new CreditCardDto
                {
                    Id = c.Id,
                    CardNumber = c.CardNumber,
                    ExpiryDate = c.ExpiryDate,
                    CVV = c.CVV,
                    ApprovedAmount = c.ApprovedAmount,
                    LoanId = c.LoanId
                }).ToList() ?? new List<CreditCardDto>()
            };
        }
    }
}