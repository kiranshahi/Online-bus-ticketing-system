using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class RouteMap : EntityTypeConfiguration<Route>
    {
        public RouteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Departure)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Arrival)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Through)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Route");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Departure).HasColumnName("Departure");
            this.Property(t => t.Arrival).HasColumnName("Arrival");
            this.Property(t => t.Through).HasColumnName("Through");
            this.Property(t => t.Fair).HasColumnName("Fair");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
