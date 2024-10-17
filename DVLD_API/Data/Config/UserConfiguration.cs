using DVLD_API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_API.Data.Config
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.Property(e => e.UserId).HasColumnName("UserID");
			builder.Property(e => e.Password).HasMaxLength(64);
			builder.Property(e => e.PersonId).HasColumnName("PersonID");
			builder.Property(e => e.Username).HasMaxLength(20);

			builder.HasOne(d => d.Person).WithOne(p => p.User)
				.HasForeignKey<User>(u => u.PersonId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK_Users_People");
		}
	}
}
