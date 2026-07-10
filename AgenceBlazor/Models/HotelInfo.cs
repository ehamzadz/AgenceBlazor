// Models/HotelInfo.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("hotels_info")]
    public class HotelInfo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Column("location")]
        [StringLength(255)]
        public string Location { get; set; }

        [Column("distance_from_haram")]
        [StringLength(100)]
        public string DistanceFromHaram { get; set; }

        [Column("client_name")]
        [StringLength(255)]
        public string ClientName { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}