using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("trips")]
    public class Trip
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("trip_name")]
        public string TripName { get; set; }

        [Column("trip_number")]
        public string TripNumber { get; set; }

        [Column("departure_date")]
        public DateTime DepartureDate { get; set; }

        [Column("trip_type")]
        public string TripType { get; set; }

        [Column("total_seats")]
        public int TotalSeats { get; set; }

        [Column("filled_seats")]
        public int FilledSeats { get; set; }

        [Column("remaining_seats")]
        public int RemainingSeats { get; set; }

        [Column("airline")]
        public string Airline { get; set; }

        [Column("departure_from")]
        public string DepartureFrom { get; set; }

        [Column("arrival_to")]
        public string ArrivalTo { get; set; }

        [Column("program")]
        public string Program { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}