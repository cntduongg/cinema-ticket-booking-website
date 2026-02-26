using BookingService.Models;
using BookingService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BookingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllAsync();
            return Ok(transactions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionsById(int id)
        {
            var transactions = await _transactionService.GetByIdAsync(id);
            return Ok(transactions);
        }

        [HttpGet("/GetTransactionByDate")]
        public async Task<IActionResult> GetTransactionByDate([FromQuery] string date)
        {
            if (!DateTime.TryParseExact(date, "dd,MM,yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest(new { message = "Invalid date format. Use dd,MM,yyyy" });
            }

            try
            {
                var transactions = await _transactionService.GetByDateAsync(parsedDate);
                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound(new { message = "No transactions found for the specified date." });
                }
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                var createdTransaction = await _transactionService.AddAsync(transaction);
                return Ok(createdTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                var updatedTransaction = await _transactionService.UpdateAsync(transaction);
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var result = await _transactionService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new { message = "Transaction deleted successfully." });
                }
                return NotFound(new { message = "Delete Transaction Error" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}