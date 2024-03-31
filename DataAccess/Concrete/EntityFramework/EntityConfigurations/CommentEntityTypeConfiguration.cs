
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
        public class CommentEntityTypeConfiguration : BaseEntityTypeConfiguration<Comment>
        {
                public override void Configure(EntityTypeBuilder<Comment> builder)
                {
                        base.Configure(builder);

                        builder.HasOne<Photo>(c => c.Photo)
                                .WithMany(p => p.Comments)
                           .OnDelete(DeleteBehavior.NoAction);
                        builder.HasOne<User>(c => c.User)
                                .WithMany(u => u.Comments)
                                .OnDelete(DeleteBehavior.NoAction);
                }
        }
}