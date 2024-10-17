using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class LicenseClassConfiguration : IEntityTypeConfiguration<LicenseClass>
	{
		public void Configure(EntityTypeBuilder<LicenseClass> builder)
		{
			builder.ToTable("LicenseClasses");

			builder.Property(e => e.LicenseClassId).HasColumnName("LicenseClassID");
			builder.Property(e => e.ClassDescription).HasMaxLength(500);
			builder.Property(e => e.ClassFees).HasColumnType("smallmoney");
			builder.Property(e => e.ClassName).HasMaxLength(50);
			builder.Property(e => e.DefaultValidityLength)
				.HasDefaultValue((byte)1)
				.HasComment("How many years the licesnse will be valid.");
			builder.Property(e => e.MinimumAllowedAge)
				.HasDefaultValue((byte)18)
				.HasComment("Minmum age allowed to apply for this license");

		}
	}
}
