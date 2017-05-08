using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class BusMap : EntityTypeConfiguration<Bus>
    {
        public BusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Bus_No)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Facilities)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.BusImage)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Bus");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Bus_No).HasColumnName("Bus_No");
            this.Property(t => t.NoOfSeats).HasColumnName("NoOfSeats");
            this.Property(t => t.Travel).HasColumnName("Travel");
            this.Property(t => t.Facilities).HasColumnName("Facilities");
            this.Property(t => t.BusImage).HasColumnName("BusImage");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
