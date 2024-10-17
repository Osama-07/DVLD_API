using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class DetainedLicenseConfiguration : IEntityTypeConfiguration<DetainedLicense>
	{
		public void Configure(EntityTypeBuilder<DetainedLicense> builder)
		{
			builder.HasKey(e => e.DetainId);

			builder.Property(e => e.DetainId).HasColumnName("DetainID");
			builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
			builder.Property(e => e.DetainDate).HasColumnType("smalldatetime");
			builder.Property(e => e.FineFees).HasColumnType("smallmoney");
			builder.Property(e => e.LicenseId).HasColumnName("LicenseID");
			builder.Property(e => e.ReleaseApplicationId).HasColumnName("ReleaseApplicationID");
			builder.Property(e => e.ReleaseDate).HasColumnType("smalldatetime");
			builder.Property(e => e.ReleasedByUserId).HasColumnName("ReleasedByUserID");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.DetainedLicenseCreatedByUsers)
				.HasForeignKey(d => d.CreatedByUserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_DetainedLicenses_Users");

			builder.HasOne(d => d.License).WithMany(p => p.DetainedLicenses)
				.HasForeignKey(d => d.LicenseId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_DetainedLicenses_Licenses");

			builder.HasOne(d => d.ReleaseApplication).WithMany(p => p.DetainedLicenses)
				.HasForeignKey(d => d.ReleaseApplicationId)
				.HasConstraintName("FK_DetainedLicenses_Applications");

			builder.HasOne(d => d.ReleasedByUser).WithMany(p => p.DetainedLicenseReleasedByUsers)
				.HasForeignKey(d => d.ReleasedByUserId)
				.HasConstraintName("FK_DetainedLicenses_Users1");

			builder.ToTable("DetainedLicenses");
		}
	}


}
