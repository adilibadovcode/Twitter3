using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Entities;

namespace Twitter.DAL.Configurations;
public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    void IEntityTypeConfiguration<AppUser>.Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(32);
        builder.Property(t => t.Surname).IsRequired().HasMaxLength(32);
        builder.Property(t => t.UserName).IsRequired().HasMaxLength(64);
        builder.Property(t => t.Email).IsRequired();
    }
}
