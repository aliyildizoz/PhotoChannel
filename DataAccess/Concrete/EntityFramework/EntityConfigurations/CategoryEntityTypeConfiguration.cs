
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Dal.EntityFramework.EntityConfigurations
{
    public class CategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.HasData(new List<Category>
            {
                new Category {Id=1,Name = "Kitap"   },
                new Category {Id=2,Name = "Sinema"  },
                new Category {Id=3,Name = "Bilim"   },
                new Category {Id=4,Name = "Kültür"  },
                new Category {Id=5,Name = "Edebiyat"}
            });
            builder.ToTable("Categories", t => t.HasTrigger("CategoriesDeleteTrigger"));

        }
    }
}