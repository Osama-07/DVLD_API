using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
	{
		public void Configure(EntityTypeBuilder<Application> builder)
		{
			builder.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
			builder.Property(e => e.ApplicantPersonId).HasColumnName("ApplicantPersonID");
			builder.Property(e => e.ApplicationDate).HasColumnType("datetime");
			builder.Property(e => e.ApplicationStatus)
				.HasDefaultValue((byte)1)
				.HasComment("1-New 2-Cancelled 3-Completed");

			builder.Property(e => e.ApplicationTypeId).HasColumnName("ApplicationTypeID");
			builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
			builder.Property(e => e.LastStatusDate).HasColumnType("datetime");
			builder.Property(e => e.PaidFees).HasColumnType("smallmoney");

			builder.HasOne(d => d.ApplicantPerson).WithMany(p => p.Applications)
				.HasForeignKey(d => d.ApplicantPersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Applications_People");

			builder.HasOne(d => d.ApplicationType).WithMany(p => p.Applications)
				.HasForeignKey(d => d.ApplicationTypeId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Applications_ApplicationTypes");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.Applications)
				.HasForeignKey(d => d.CreatedByUserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Applications_Users");

			builder.ToTable("Applications");
		}
	}

}
