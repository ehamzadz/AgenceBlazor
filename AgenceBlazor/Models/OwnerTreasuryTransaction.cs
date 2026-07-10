using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    public class OwnerTreasuryTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // deposit, withdrawal, transfer_to_main, transfer_from_main

        [Required]
        [StringLength(20)]
        public string Source { get; set; } = string.Empty; // bank, cash

        [Required]
        [Column(TypeName = "numeric(12,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        [StringLength(30)]
        public string? TransferType { get; set; }

        public int? MainTreasuryRefId { get; set; }

        [ForeignKey("MainTreasuryRefId")]
        public TreasuryTransaction? MainTreasuryTransaction { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}