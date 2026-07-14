// Models/TripAirlinePricing.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenceBlazor.Models
{
    [Table("trip_airline_pricing")]
    public class TripAirlinePricing
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("trip_id")]
        [Required]
        public int TripId { get; set; }

        [ForeignKey("TripId")]
        public Trip? Trip { get; set; }

        // ============ PRICES ============

        [Column("adult_price")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "سعر البالغ يجب أن يكون أكبر من 0")]
        [Display(Name = "سعر البالغ")]
        public decimal AdultPrice { get; set; }

        [Column("child_price")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "سعر الطفل يجب أن يكون أكبر من 0")]
        [Display(Name = "سعر الطفل")]
        public decimal ChildPrice { get; set; }

        [Column("infant_price")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "سعر الرضيع يجب أن يكون أكبر من 0")]
        [Display(Name = "سعر الرضيع")]
        public decimal InfantPrice { get; set; }

        // ============ FREE SEATS ============

        [Column("free_seats_count")]
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "عدد المقاعد المجانية")]
        public int FreeSeatsCount { get; set; }

        [Column("free_seat_price")]
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "سعر المقعد الرمزي")]
        public decimal FreeSeatPrice { get; set; } = 18500;

        // ============ PASSENGER COUNTS ============

        [Column("adult_count")]
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "عدد البالغين")]
        public int AdultCount { get; set; }

        [Column("child_count")]
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "عدد الأطفال")]
        public int ChildCount { get; set; }

        [Column("infant_count")]
        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "عدد الرضع")]
        public int InfantCount { get; set; }

        // ============ COMPUTED TOTALS (Readonly) ============

        [Column("total_passengers")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي الركاب")]
        public int TotalPassengers { get; set; }

        [Column("adult_total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي البالغين")]
        public decimal AdultTotal { get; set; }

        [Column("child_total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي الأطفال")]
        public decimal ChildTotal { get; set; }

        [Column("infant_total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي الرضع")]
        public decimal InfantTotal { get; set; }

        [Column("free_seats_total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي المقاعد الرمزية")]
        public decimal FreeSeatsTotal { get; set; }

        [Column("total_airline_cost")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "إجمالي تكلفة الخطوط")]
        public decimal TotalAirlineCost { get; set; }

        // ============ METADATA ============

        [Column("notes")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Column("is_paid")]
        [Display(Name = "تم الدفع")]
        public bool IsPaid { get; set; } = false;

        [Column("paid_date")]
        [Display(Name = "تاريخ الدفع")]
        public DateTime? PaidDate { get; set; }

        [Column("paid_amount")]
        [Range(0, double.MaxValue)]
        [Display(Name = "المبلغ المدفوع")]
        public decimal PaidAmount { get; set; }

        [Column("created_at")]
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        [Display(Name = "تاريخ التحديث")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ============ COMPUTED PROPERTIES (Not mapped to DB) ============

        [NotMapped]
        [Display(Name = "المبلغ المتبقي")]
        public decimal RemainingAmount => TotalAirlineCost - PaidAmount;

        [NotMapped]
        [Display(Name = "نسبة الدفع")]
        public decimal PaymentPercentage => TotalAirlineCost > 0
            ? Math.Round((PaidAmount / TotalAirlineCost) * 100, 1)
            : 0;

        [NotMapped]
        [Display(Name = "عدد الركاب الفعلي")]
        public int ActualPassengers => AdultCount + ChildCount + InfantCount;

        [NotMapped]
        [Display(Name = "إجمالي المقاعد المستخدمة")]
        public int TotalUsedSeats => AdultCount + ChildCount + InfantCount + FreeSeatsCount;

        [NotMapped]
        public string PaymentStatus => IsPaid ? "✅ مدفوع" :
            PaidAmount > 0 ? "⚠️ جزئي" : "❌ غير مدفوع";

        [NotMapped]
        public string PaymentStatusClass => IsPaid ? "paid" :
            PaidAmount > 0 ? "partial" : "pending";

        // ============ HELPER METHODS ============

        /// <summary>
        /// Calculate total manually (before saving to DB)
        /// </summary>
        public decimal CalculateTotal()
        {
            return (AdultCount * AdultPrice) +
                   (ChildCount * ChildPrice) +
                   (InfantCount * InfantPrice) +
                   (FreeSeatsCount * FreeSeatPrice);
        }

        /// <summary>
        /// Validate that total passengers don't exceed trip seats
        /// </summary>
        public bool ValidatePassengerCount(int tripTotalSeats)
        {
            return TotalUsedSeats <= tripTotalSeats;
        }
    }
}