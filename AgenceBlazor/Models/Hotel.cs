// Models/Hotel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("hotels")]
    public class Hotel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("trip_id")]
        public int TripId { get; set; }

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Column("group_price")]
        public decimal GroupPrice { get; set; }

        [Column("quadruple_price")]
        public decimal QuadruplePrice { get; set; }

        [Column("triple_price")]
        public decimal TriplePrice { get; set; }

        [Column("double_price")]
        public decimal DoublePrice { get; set; }

        [Column("child_price")]
        public decimal ChildPrice { get; set; }

        [Column("infant_price")]
        public decimal InfantPrice { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }
    }
}