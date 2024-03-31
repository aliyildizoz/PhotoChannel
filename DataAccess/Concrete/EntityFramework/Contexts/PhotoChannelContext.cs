using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Entities.Concrete;
using DataAccess.Dal.EntityFramework.EntityConfigurations;

namespace DataAccess.Dal.EntityFramework.Contexts
{
    public class PhotoChannelContext : DbContext
    {
        public PhotoChannelContext(DbContextOptions<PhotoChannelContext> options) : base(options)
        {

        }
        public PhotoChannelContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LikeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperationClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserOperationClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriberEntityTypeConfiguration());

            modelBuilder.Entity<Category>().HasData(new List<Category>
            {
                new Category {Id=1,Name = "Kitap"   },
                new Category {Id=2,Name = "Sinema"  },
                new Category {Id=3,Name = "Bilim"   },
                new Category {Id=4,Name = "Kültür"  },
                new Category {Id=5,Name = "Edebiyat"}
            });
            modelBuilder.Entity<OperationClaim>().HasData(new List<OperationClaim>
            {
                new OperationClaim {Id=1, ClaimName = "Admin"},
                new OperationClaim {Id=2, ClaimName = "Users"}
            });

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<ChannelCategory> ChannelCategories { get; set; }
    }
}
