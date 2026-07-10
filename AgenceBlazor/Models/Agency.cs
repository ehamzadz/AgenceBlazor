using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("agencies")]
    public class Agency
    {
        [Key]
        [Column("agencyid")]
        public Guid AgencyId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("agencyname")]
        [StringLength(200)]
        public string AgencyName { get; set; }

        [Column("agencytype")]
        [StringLength(100)]
        public string AgencyType { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Column("commissionrate")]
        public decimal CommissionRate { get; set; }

        [Column("contractdate")]
        public DateTime? ContractDate { get; set; }

        [Column("pilgrimscount")]
        public int PilgrimsCount { get; set; }

        [Column("debtamount")]
        public decimal DebtAmount { get; set; }

        [Column("paidamount")]
        public decimal PaidAmount { get; set; }

        [Column("remainingamount")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal RemainingAmount { get; set; }

        [Column("phone")]
        [StringLength(30)]
        public string? Phone { get; set; }

        [Column("email")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string City { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedat")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}