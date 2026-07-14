using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AgenceBlazor.Models;

namespace AgenceBlazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<AgencyBooking> AgencyBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<HotelInfo> HotelInfos { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<TreasuryTransaction> TreasuryTransactions { get; set; }
        public DbSet<OwnerTreasuryTransaction> OwnerTreasuryTransactions { get; set; }
        public DbSet<DirectPilgrim> DirectPilgrims { get; set; }
        public DbSet<DirectPilgrimFamily> DirectPilgrimFamilies { get; set; }
        public DbSet<TripGuide> TripGuides { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<TripAirlinePricing> TripAirlinePricings { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Trips table
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("trips");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripName).HasColumnName("trip_name");
                entity.Property(e => e.TripNumber).HasColumnName("trip_number");

                entity.Property(e => e.DepartureDate)
                    .HasColumnName("departure_date")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.TripType).HasColumnName("trip_type");
                entity.Property(e => e.TotalSeats).HasColumnName("total_seats");
                entity.Property(e => e.FilledSeats).HasColumnName("filled_seats");
                entity.Property(e => e.RemainingSeats)
                    .HasColumnName("remaining_seats")
                    .HasComputedColumnSql("total_seats - filled_seats", stored: true);
                entity.Property(e => e.Airline).HasColumnName("airline");
                entity.Property(e => e.DepartureFrom).HasColumnName("departure_from");
                entity.Property(e => e.ArrivalTo).HasColumnName("arrival_to");
                entity.Property(e => e.Program).HasColumnName("program");
                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.HasQueryFilter(e => e.IsActive);
            });

            // Agencies table
            modelBuilder.Entity<Agency>(entity =>
            {
                entity.ToTable("agencies");
                entity.HasKey(e => e.AgencyId);

                entity.Property(e => e.AgencyId)
                    .HasColumnName("agencyid")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.AgencyName)
                    .HasColumnName("agencyname")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.AgencyType)
                    .HasColumnName("agencytype")
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");

                entity.Property(e => e.CommissionRate)
                    .HasColumnName("commissionrate")
                    .HasDefaultValue(0);

                entity.Property(e => e.ContractDate)
                    .HasColumnName("contractdate")
                    .HasConversion(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
                    );

                entity.Property(e => e.PilgrimsCount)
                    .HasColumnName("pilgrimscount")
                    .HasDefaultValue(0);

                entity.Property(e => e.DebtAmount)
                    .HasColumnName("debtamount")
                    .HasDefaultValue(0);

                entity.Property(e => e.PaidAmount)
                    .HasColumnName("paidamount")
                    .HasDefaultValue(0);

                entity.Property(e => e.RemainingAmount)
                    .HasColumnName("remainingamount")
                    .HasComputedColumnSql("debtamount - paidamount", stored: true);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(30);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(150);

                entity.Property(e => e.Address)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdat")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedat")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
            });

            // In OnModelCreating:
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("hotels");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("trip_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.GroupPrice).HasColumnName("group_price");
                entity.Property(e => e.QuadruplePrice).HasColumnName("quadruple_price");
                entity.Property(e => e.TriplePrice).HasColumnName("triple_price");
                entity.Property(e => e.DoublePrice).HasColumnName("double_price");
                entity.Property(e => e.ChildPrice).HasColumnName("child_price");
                entity.Property(e => e.InfantPrice).HasColumnName("infant_price");
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasQueryFilter(e => e.IsActive);
            });
            // AgencyBookings table
            modelBuilder.Entity<AgencyBooking>(entity =>
            {
                entity.ToTable("agency_bookings");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("trip_id");
                entity.Property(e => e.AgencyId).HasColumnName("agency_id");
                entity.Property(e => e.AgencyName).HasColumnName("agency_name");
                entity.Property(e => e.HotelName).HasColumnName("hotel_name");

                entity.Property(e => e.GroupCount).HasColumnName("group_count");
                entity.Property(e => e.QuadrupleCount).HasColumnName("quadruple_count");
                entity.Property(e => e.TripleCount).HasColumnName("triple_count");
                entity.Property(e => e.DoubleCount).HasColumnName("double_count");
                entity.Property(e => e.ChildCount).HasColumnName("child_count");
                entity.Property(e => e.InfantCount).HasColumnName("infant_count");

                entity.Property(e => e.GroupPrice).HasColumnName("group_price");
                entity.Property(e => e.QuadruplePrice).HasColumnName("quadruple_price");
                entity.Property(e => e.TriplePrice).HasColumnName("triple_price");
                entity.Property(e => e.DoublePrice).HasColumnName("double_price");
                entity.Property(e => e.ChildPrice).HasColumnName("child_price");
                entity.Property(e => e.InfantPrice).HasColumnName("infant_price");

                entity.Property(e => e.TotalPilgrims)
                    .HasColumnName("total_pilgrims")
                    .HasComputedColumnSql("group_count + quadruple_count + triple_count + double_count + child_count + infant_count", stored: true);

                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
                entity.Property(e => e.Commission).HasColumnName("commission");
                entity.Property(e => e.Reduction).HasColumnName("reduction");

                entity.Property(e => e.NetProfit)
                    .HasColumnName("net_profit")
                    .HasComputedColumnSql("total_amount - commission", stored: true);

                // ADD THESE NEW MAPPINGS
                entity.Property(e => e.PaidAmount).HasColumnName("paid_amount");
                entity.Property(e => e.RemainingAmount)
                    .HasColumnName("remaining_amount")
                    .HasComputedColumnSql("total_amount - commission - paid_amount", stored: true);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
                entity.Property(e => e.DirectPilgrimId).HasColumnName("direct_pilgrim_id");
            });


            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AgencyId).HasColumnName("agency_id");
                entity.Property(e => e.BookingId).HasColumnName("booking_id");  // ADD THIS
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
            });

            modelBuilder.Entity<HotelInfo>(entity =>
            {
                entity.ToTable("hotels_info");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.DistanceFromHaram).HasColumnName("distance_from_haram");
                entity.Property(e => e.ClientName).HasColumnName("client_name");
                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("expenses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.TripId).HasColumnName("trip_id");
                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.ExpenseDate)
                    .HasColumnName("expense_date")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
            });

            modelBuilder.Entity<ExpenseCategory>(entity =>
            {
                entity.ToTable("expense_categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });


            modelBuilder.Entity<TreasuryTransaction>(entity =>
            {
                entity.ToTable("treasury");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ReferenceType).HasColumnName("reference_type");
                entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                // Add these mappings
                entity.Property(e => e.OwnerTreasuryRefId).HasColumnName("owner_treasury_ref_id");
                entity.Property(e => e.TransferType).HasColumnName("transfer_type");

                // Relationship
                entity.HasOne(t => t.OwnerTreasuryTransaction)
                    .WithMany()
                    .HasForeignKey(t => t.OwnerTreasuryRefId)
                    .HasConstraintName("fk_owner_treasury_ref")
                    .OnDelete(DeleteBehavior.SetNull);
            });
            // Owner Treasury configuration
            modelBuilder.Entity<OwnerTreasuryTransaction>(entity =>
            {
                entity.ToTable("owner_treasury");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Source).HasColumnName("source");
                entity.Property(e => e.Amount).HasColumnName("amount");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.TransactionDate).HasColumnName("transaction_date")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.TransferType).HasColumnName("transfer_type");
                entity.Property(e => e.MainTreasuryRefId).HasColumnName("main_treasury_ref_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.HasIndex(e => e.TransactionDate).HasDatabaseName("idx_owner_treasury_date");
                entity.HasIndex(e => e.Type).HasDatabaseName("idx_owner_treasury_type");
                entity.HasIndex(e => e.Source).HasDatabaseName("idx_owner_treasury_source");
                entity.HasIndex(e => e.MainTreasuryRefId).HasDatabaseName("idx_owner_treasury_main_ref");

                // Relationship
                entity.HasOne(e => e.MainTreasuryTransaction)
                    .WithMany()
                    .HasForeignKey(e => e.MainTreasuryRefId)
                    .HasConstraintName("fk_main_treasury_ref")
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<DirectPilgrim>(entity =>
            {
                entity.ToTable("direct_pilgrims");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("trip_id");
                entity.Property(e => e.MainPilgrimName).HasColumnName("main_pilgrim_name");
                entity.Property(e => e.MainPilgrimPhone).HasColumnName("main_pilgrim_phone");
                entity.Property(e => e.MainPilgrimAddress).HasColumnName("main_pilgrim_address");
                entity.Property(e => e.MainPilgrimRoomType).HasColumnName("main_pilgrim_room_type");
                entity.Property(e => e.HotelName).HasColumnName("hotel_name");
                entity.Property(e => e.TotalPilgrims).HasColumnName("total_pilgrims");
                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
                entity.Property(e => e.Discount).HasColumnName("discount");
                entity.Property(e => e.NetAmount).HasColumnName("net_amount");
                entity.Property(e => e.PaidAmount).HasColumnName("paid_amount");
                entity.Property(e => e.RemainingAmount).HasColumnName("remaining_amount")
                    .HasComputedColumnSql("net_amount - paid_amount", stored: true);
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasMany(e => e.FamilyMembers)
                    .WithOne(f => f.DirectPilgrim)
                    .HasForeignKey(f => f.DirectPilgrimId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DirectPilgrimFamily>(entity =>
            {
                entity.ToTable("direct_pilgrim_family");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DirectPilgrimId).HasColumnName("direct_pilgrim_id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Relation).HasColumnName("relation");
                entity.Property(e => e.RoomType).HasColumnName("room_type");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            });


            modelBuilder.Entity<TripGuide>(entity =>
            {
                // Use lowercase table name for PostgreSQL
                entity.ToTable("tripguides");

                // Map properties to lowercase columns
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("tripid");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Agency).HasColumnName("agency").HasMaxLength(200);
                entity.Property(e => e.GrantAmount).HasColumnName("grantamount").HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedat");

                // Configure relationship
                entity.HasOne(e => e.Trip)
                      .WithMany()
                      .HasForeignKey(e => e.TripId)
                      .HasConstraintName("tripguides_tripid_fkey")
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<TripAirlinePricing>(entity =>
            {
                entity.ToTable("trip_airline_pricing");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TripId).HasColumnName("trip_id");
                entity.Property(e => e.AdultPrice).HasColumnName("adult_price");
                entity.Property(e => e.ChildPrice).HasColumnName("child_price");
                entity.Property(e => e.InfantPrice).HasColumnName("infant_price");
                entity.Property(e => e.FreeSeatsCount).HasColumnName("free_seats_count");
                entity.Property(e => e.FreeSeatPrice).HasColumnName("free_seat_price");
                entity.Property(e => e.AdultCount).HasColumnName("adult_count");
                entity.Property(e => e.ChildCount).HasColumnName("child_count");
                entity.Property(e => e.InfantCount).HasColumnName("infant_count");

                // Computed columns (readonly)
                entity.Property(e => e.TotalPassengers)
                    .HasColumnName("total_passengers")
                    .HasComputedColumnSql("adult_count + child_count + infant_count + free_seats_count", stored: true);

                entity.Property(e => e.AdultTotal)
                    .HasColumnName("adult_total")
                    .HasComputedColumnSql("adult_count * adult_price", stored: true);

                entity.Property(e => e.ChildTotal)
                    .HasColumnName("child_total")
                    .HasComputedColumnSql("child_count * child_price", stored: true);

                entity.Property(e => e.InfantTotal)
                    .HasColumnName("infant_total")
                    .HasComputedColumnSql("infant_count * infant_price", stored: true);

                entity.Property(e => e.FreeSeatsTotal)
                    .HasColumnName("free_seats_total")
                    .HasComputedColumnSql("free_seats_count * free_seat_price", stored: true);

                entity.Property(e => e.TotalAirlineCost)
                    .HasColumnName("total_airline_cost")
                    .HasComputedColumnSql("(adult_count * adult_price) + (child_count * child_price) + (infant_count * infant_price) + (free_seats_count * free_seat_price)", stored: true);

                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.Property(e => e.IsPaid).HasColumnName("is_paid");
                entity.Property(e => e.PaidDate).HasColumnName("paid_date");
                entity.Property(e => e.PaidAmount).HasColumnName("paid_amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                // Relationships
                entity.HasOne(e => e.Trip)
                    .WithMany()
                    .HasForeignKey(e => e.TripId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}