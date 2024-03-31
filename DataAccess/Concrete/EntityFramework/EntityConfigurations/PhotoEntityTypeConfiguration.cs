
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class PhotoEntityTypeConfiguration : BaseEntityTypeConfiguration<Photo>
    {
        public override void Configure(EntityTypeBuilder<Photo> builder)
        {
            base.Configure(builder);

            builder.ToTable("Photos", t => t.HasTrigger("PhotosDeleteTrigger"));

            builder.HasOne<User>(photo => photo.User)
                    .WithMany(user => user.Photos)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<Channel>(photo => photo.Channel)
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}