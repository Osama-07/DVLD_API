using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class PersonConfiguration : IEntityTypeConfiguration<Person>
	{
		public void Configure(EntityTypeBuilder<Person> builder)
		{
			builder.HasKey(e => e.PersonId).HasName("PK_People");

			builder.ToTable("Pepole");

			builder.Property(e => e.PersonId).HasColumnName("PersonID");
			builder.Property(e => e.Address).HasMaxLength(500);
			builder.Property(e => e.CountryId).HasColumnName("CountryID");
			builder.Property(e => e.DateOfBirth).HasColumnType("datetime");
			builder.Property(e => e.Email).HasMaxLength(50);
			builder.Property(e => e.FirstName).HasMaxLength(20);
			builder.Property(e => e.Gender).HasComment("0 Male , 1 Femail");
			builder.Property(e => e.LastName).HasMaxLength(20);
			builder.Property(e => e.NationalNo).HasMaxLength(20);
			builder.Property(e => e.PersonalPicture).HasMaxLength(250);
			builder.Property(e => e.Phone).HasMaxLength(20);
			builder.Property(e => e.SecondName).HasMaxLength(20);
			builder.Property(e => e.ThirdName).HasMaxLength(20);

			builder.HasOne(d => d.Country)
				  .WithMany(p => p.People)
				  .HasForeignKey(d => d.CountryId)
				  .OnDelete(DeleteBehavior.SetNull)
				  .HasConstraintName("FK_People_Countries1");
		}
	}
}
