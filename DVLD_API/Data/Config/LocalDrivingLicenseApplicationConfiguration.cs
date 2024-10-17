using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class LocalDrivingLicenseApplicationConfiguration : IEntityTypeConfiguration<LocalDrivingLicenseApplication>
	{
		public void Configure(EntityTypeBuilder<LocalDrivingLicenseApplication> builder)
		{
			builder.HasKey(e => e.LocalDrivingLicenseApplicationId).HasName("PK_DrivingLicsenseApplications");

			builder.Property(e => e.LocalDrivingLicenseApplicationId).HasColumnName("LocalDrivingLicenseApplicationID");
			builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
			builder.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");

			builder.HasOne(d => d.Application).WithMany(p => p.LocalDrivingLicenseApplications)
				.HasForeignKey(d => d.ApplicationId)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("FK_DrivingLicsenseApplications_Applications");

			builder.HasOne(d => d.LicenseClass).WithMany(p => p.LocalDrivingLicenseApplications)
				.HasForeignKey(d => d.LicenseClassId)
				.OnDelete(DeleteBehavior.SetNull)
				.HasConstraintName("FK_DrivingLicsenseApplications_LicenseClasses");

			builder.ToTable("LocalDrivingLicenseApplications");
		}
	}
}
