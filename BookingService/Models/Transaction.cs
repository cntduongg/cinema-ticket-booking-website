using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace BookingService.Models;

public class Transaction
{
    public int Id { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int PaymentId { get; set; }
    public DateOnly TransactionDate { get; set; }
    public int Price { get; set; }
    public bool Status { get; set; }

    [Required]
    [MaxLength(100)] 
    public string TransactionCode { get; set; } = null!;

    public Transaction(int orderId, int paymentId, DateOnly transactionDate, int price, bool status, string transactionCode)
    {
        OrderId = orderId;
        PaymentId = paymentId;
        TransactionDate = transactionDate;
        Price = price;
        Status = status;
        TransactionCode = transactionCode;
    }

    public Transaction()
    {
    }
}