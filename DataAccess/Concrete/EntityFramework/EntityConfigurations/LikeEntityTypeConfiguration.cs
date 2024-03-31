
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class LikeEntityTypeConfiguration : BaseEntityTypeConfiguration<Like>
    {
        public override void Configure(EntityTypeBuilder<Like> builder)
        {
            base.Configure(builder);

            builder.HasOne<User>(l => l.User)
                    .WithMany(u => u.Likes)
                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Photo>(l => l.Photo)
                    .WithMany(u => u.Likes)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}