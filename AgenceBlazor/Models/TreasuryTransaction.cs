using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    public class TreasuryTransaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string? ReferenceType { get; set; }    // Make nullable
        public int? ReferenceId { get; set; }          // Make nullable
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }             // Make nullable
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? TransferType { get; set; }

        public int? OwnerTreasuryRefId { get; set; }

        [ForeignKey("OwnerTreasuryRefId")]
        public OwnerTreasuryTransaction? OwnerTreasuryTransaction { get; set; }
    }
}