namespace AgenceBlazor.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public int? TripId { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}