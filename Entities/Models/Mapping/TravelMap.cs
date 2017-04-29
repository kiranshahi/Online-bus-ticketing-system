using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class TravelMap : EntityTypeConfiguration<Travel>
    {
        public TravelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Travel_Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Travel_Detail)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Travel");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Travel_Name).HasColumnName("Travel_Name");
            this.Property(t => t.Travel_Detail).HasColumnName("Travel_Detail");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
