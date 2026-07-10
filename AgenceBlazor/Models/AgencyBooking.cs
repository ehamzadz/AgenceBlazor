// Models/AgencyBooking.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("agency_bookings")]
    public class AgencyBooking
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("trip_id")]
        public int TripId { get; set; }

        [Column("agency_id")]
        public Guid? AgencyId { get; set; }

        [Column("agency_name")]
        [StringLength(255)]
        public string AgencyName { get; set; }

        [Column("hotel_name")]
        [StringLength(255)]
        public string HotelName { get; set; }

        [Column("group_count")]
        public int GroupCount { get; set; }

        [Column("quadruple_count")]
        public int QuadrupleCount { get; set; }

        [Column("triple_count")]
        public int TripleCount { get; set; }

        [Column("double_count")]
        public int DoubleCount { get; set; }

        [Column("child_count")]
        public int ChildCount { get; set; }

        [Column("infant_count")]
        public int InfantCount { get; set; }

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

        [Column("total_pilgrims")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int TotalPilgrims { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("commission")]
        public decimal Commission { get; set; }

        [Column("reduction")]
        public decimal Reduction { get; set; }

        [Column("net_profit")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal NetProfit { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "confirmed";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("TripId")]
        public Trip Trip { get; set; }

        [ForeignKey("AgencyId")]
        public Agency Agency { get; set; }
    }
}