using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LoanManagementSystem.Controllers
{
    [Route("api/credit-cards")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardRepository _creditCardRepository;

        public CreditCardController(ICreditCardRepository creditCardRepository)
        {
            _creditCardRepository = creditCardRepository;
        }

        // ✅ Get All Credit Cards for a User
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCreditCardsByUserId(int userId)
        {
            var creditCards = await _creditCardRepository.GetCreditCardsByUserId(userId);
            return Ok(creditCards);
        }

        // ✅ Get a Single Credit Card by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCreditCardById(int id)
        {
            var creditCard = await _creditCardRepository.GetCreditCardById(id);
            if (creditCard == null) return NotFound("Credit card not found.");
            return Ok(creditCard);
        }

        // ✅ Delete a Credit Card
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreditCard(int id)
        {
            var creditCard = await _creditCardRepository.GetCreditCardById(id);
            if (creditCard == null) return NotFound("Credit card not found.");

            _creditCardRepository.DeleteCreditCard(creditCard);
            await _creditCardRepository.SaveChanges();

            return Ok("Credit card deleted successfully.");
        }
    }
}
