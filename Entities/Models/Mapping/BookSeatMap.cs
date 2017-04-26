using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class BookSeatMap : EntityTypeConfiguration<BookSeat>
    {
        public BookSeatMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Seats)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("BookSeats");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Datetime).HasColumnName("Datetime");
            this.Property(t => t.ScheduleId).HasColumnName("ScheduleId");
            this.Property(t => t.Seats).HasColumnName("Seats");
        }
    }
}
