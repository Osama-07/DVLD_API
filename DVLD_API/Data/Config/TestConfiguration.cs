using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class TestConfiguration : IEntityTypeConfiguration<Test>
	{
		public void Configure(EntityTypeBuilder<Test> builder)
		{
			builder.ToTable("Tests");

			builder.Property(e => e.TestId).HasColumnName("TestID");
			builder.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");
			builder.Property(e => e.Notes).HasMaxLength(500);
			builder.Property(e => e.TestAppointmentId).HasColumnName("TestAppointmentID");
			builder.Property(e => e.TestResult).HasComment("0 - Fail 1-Pass");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.Tests)
				.HasForeignKey(d => d.CreatedByUserId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Tests_Users");

			builder.HasOne(d => d.TestAppointment).WithOne(p => p.Test)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Tests_TestAppointments");
		}
	}
}
