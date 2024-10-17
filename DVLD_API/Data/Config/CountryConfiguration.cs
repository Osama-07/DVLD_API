using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class CountryConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.ToTable("Countries");

			builder.HasKey(e => e.CountryId).HasName("PK__Countrie__10D160BFDBD6933F");

			builder.Property(e => e.CountryId).HasColumnName("CountryID");
			builder.Property(e => e.CountryName).HasMaxLength(50);
		}
	}
}
