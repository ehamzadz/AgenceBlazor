using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    public class DirectPilgrim
    {
        [Key]
        public int Id { get; set; }

        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip? Trip { get; set; }

        [Required]
        [StringLength(255)]
        public string MainPilgrimName { get; set; } = string.Empty;

        [StringLength(30)]
        public string? MainPilgrimPhone { get; set; }

        public string? MainPilgrimAddress { get; set; }

        [StringLength(20)]
        public string MainPilgrimRoomType { get; set; } = "ثلاثي";

        [StringLength(255)]
        public string? HotelName { get; set; }

        public int TotalPilgrims { get; set; } = 1;

        [Column(TypeName = "numeric(12,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal NetAmount { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        public decimal PaidAmount { get; set; }

        [Column(TypeName = "numeric(12,2)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal RemainingAmount { get; set; }
        public string? Notes { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "confirmed";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public List<DirectPilgrimFamily> FamilyMembers { get; set; } = new();
    }
}