using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            await _userRepository.AddUser(user);
            await _userRepository.SaveChanges();
            return Ok("User registered successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            await _userRepository.SaveChanges();

            return Ok("User updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            _userRepository.DeleteUser(user);
            await _userRepository.SaveChanges();

            return Ok("User deleted successfully.");
        }
    }
}
