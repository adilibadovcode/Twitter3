using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Twitter.Core.Entities;

namespace Twitter.DAL.Configurations;
public class TopicConfig : IEntityTypeConfiguration<Topic>
{
    void IEntityTypeConfiguration<Topic>.Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.Property(t => t.Name).IsRequired().HasMaxLength(32);
    }
}
