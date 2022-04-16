using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTE.Entities;

namespace YTE.DataAccess.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey("Id");

            builder
                .Property(b => b.UserName)
                .HasMaxLength(50)
                .IsRequired();

            builder
              .Property(b => b.Name)
              .HasMaxLength(50)
              .IsRequired();

            builder
              .Property(b => b.Pronoun)
              .HasMaxLength(50)
              .IsRequired();

            builder
              .Property(b => b.Email)
              .HasMaxLength(150)
              .IsRequired();

            builder
              .Property(b => b.PasswordHash)
              .HasMaxLength(150)
              .IsRequired();


            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}
