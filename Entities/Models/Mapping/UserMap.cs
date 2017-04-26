using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Entities.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.User_Id);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.First_name)
                .HasMaxLength(50);

            this.Property(t => t.Last_name)
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(50);

            this.Property(t => t.Contact)
                .HasMaxLength(50);

            this.Property(t => t.ProfileImage)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.User_Id).HasColumnName("User_Id");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.UserRole).HasColumnName("UserRole");
            this.Property(t => t.First_name).HasColumnName("First_name");
            this.Property(t => t.Last_name).HasColumnName("Last_name");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Contact).HasColumnName("Contact");
            this.Property(t => t.ProfileImage).HasColumnName("ProfileImage");
        }
    }
}
