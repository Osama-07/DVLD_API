using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class LicenseConfiguration : IEntityTypeConfiguration<License>
	{
		public void Configure(EntityTypeBuilder<License> builder)
		{
			builder.ToTable("Licenses");

			builder.Property(e => e.LicenseId).HasColumnName("LicenseID");
			builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
			builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
			builder.Property(e => e.DriverId).HasColumnName("DriverID");
			builder.Property(e => e.ExpirationDate).HasColumnType("datetime");
			builder.Property(e => e.IsActive).HasDefaultValue(true);
			builder.Property(e => e.IssueDate).HasColumnType("datetime");
			builder.Property(e => e.IssueReason)
				.HasDefaultValue((byte)1)
				.HasComment("1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.");
			builder.Property(e => e.Notes).HasMaxLength(500);
			builder.Property(e => e.PaidFees).HasColumnType("smallmoney");

			builder.HasOne(d => d.Application).WithMany(p => p.Licenses)
				.HasForeignKey(d => d.ApplicationId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Licenses_Applications");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.Licenses)
				.HasForeignKey(d => d.CreatedByUserId)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("FK_Licenses_Users");

			builder.HasOne(d => d.Driver).WithMany(p => p.Licenses)
				.HasForeignKey(d => d.DriverId)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("FK_Licenses_Drivers");

			builder.HasOne(d => d.LicenseClassNavigation).WithMany(p => p.Licenses)
				.HasForeignKey(d => d.LicenseClass)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Licenses_LicenseClasses");
		}
	}
}
