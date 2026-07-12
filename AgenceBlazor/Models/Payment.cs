// Models/Payment.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("agency_id")]
        public Guid AgencyId { get; set; }

        [Column("booking_id")]
        public int? BookingId { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("payment_method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = "نقداً";

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Column("notes")]
        public string Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("AgencyId")]
        public Agency Agency { get; set; }
    }
}