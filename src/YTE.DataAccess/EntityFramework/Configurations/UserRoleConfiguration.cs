using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YTE.Entities;

namespace YTE.DataAccess.EntityFramework.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UsersRoles");

            builder.HasKey(ur => new { ur.RoleId, ur.UserId});
        }
    }
}
