using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class ApplicationTypeConfiguration : IEntityTypeConfiguration<ApplicationType>
	{
		public void Configure(EntityTypeBuilder<ApplicationType> builder)
		{
			builder.ToTable("ApplicationTypes");

			builder.Property(e => e.ApplicationTypeId).HasColumnName("ApplicationTypeID");
			builder.Property(e => e.ApplicationFees).HasColumnType("smallmoney");
			builder.Property(e => e.ApplicationTypeTitle).HasMaxLength(150);
		}
	}
}
