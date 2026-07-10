using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    public class DirectPilgrimFamily
    {
        [Key]
        public int Id { get; set; }

        public int DirectPilgrimId { get; set; }

        [ForeignKey("DirectPilgrimId")]
        public DirectPilgrim? DirectPilgrim { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Relation { get; set; }

        [StringLength(20)]
        public string RoomType { get; set; } = "ثلاثي";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}