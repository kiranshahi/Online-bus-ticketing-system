using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class ScheduleMap : EntityTypeConfiguration<Schedule>
    {
        public ScheduleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DepartureTime)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Schedule");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Bus_Id).HasColumnName("Bus_Id");
            this.Property(t => t.Route_Id).HasColumnName("Route_Id");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.DepartureTime).HasColumnName("DepartureTime");
        }
    }
}
