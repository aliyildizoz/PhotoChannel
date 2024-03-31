
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class SubscriberEntityTypeConfiguration : BaseEntityTypeConfiguration<Subscriber>
    {
        public override void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>(s => s.User)
                    .WithMany(u => u.Subscribers)
                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Channel>(s => s.Channel)
                    .WithMany(u => u.Subscribers)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}