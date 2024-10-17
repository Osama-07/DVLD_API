using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class TestAppointmentConfiguration : IEntityTypeConfiguration<TestAppointment>
	{
		public void Configure(EntityTypeBuilder<TestAppointment> builder)
		{
			builder.ToTable("TestAppointments");

			builder.Property(e => e.TestAppointmentId).HasColumnName("TestAppointmentID");
			builder.Property(e => e.AppointmentDate).HasColumnType("smalldatetime");
			builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
			builder.Property(e => e.LocalDrivingLicenseApplicationId).HasColumnName("LocalDrivingLicenseApplicationID");
			builder.Property(e => e.PaidFees).HasColumnType("smallmoney");
			builder.Property(e => e.RetakeTestApplicationId).HasColumnName("RetakeTestApplicationID");
			builder.Property(e => e.TestTypeId).HasColumnName("TestTypeID");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.TestAppointments)
				.HasForeignKey(d => d.CreatedByUserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_TestAppointments_Users");

			builder.HasOne(d => d.LocalDrivingLicenseApplication).WithMany(p => p.TestAppointments)
				.HasForeignKey(d => d.LocalDrivingLicenseApplicationId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_TestAppointments_LocalDrivingLicenseApplications");

			builder.HasOne(d => d.TestType).WithMany(p => p.TestAppointments)
				.HasForeignKey(d => d.TestTypeId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_TestAppointments_TestTypes");
		}
	}

}
