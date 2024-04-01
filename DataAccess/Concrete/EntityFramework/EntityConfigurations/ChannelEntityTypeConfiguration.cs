
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class ChannelEntityTypeConfiguration : BaseEntityTypeConfiguration<Channel>
    {
        public override void Configure(EntityTypeBuilder<Channel> builder)
        {
            base.Configure(builder);

            builder.ToTable("Channels", t => t.HasTrigger("ChannelsDeleteTrigger"));

        }
    }
}