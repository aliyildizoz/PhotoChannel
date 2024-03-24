using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Concrete;
using Entities.Concrete;

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
            modelBuilder.HasDefaultSchema("PhotoChannel");
            modelBuilder.Entity<Photo>().HasOne<User>(photo => photo.User).WithMany(user => user.Photos).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Subscriber>().HasOne<User>(s => s.User).WithMany(u => u.Subscribers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Subscriber>().HasOne<Channel>(s => s.Channel).WithMany(u => u.Subscribers).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne<Photo>(c => c.Photo).WithMany(p => p.Comments).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Comment>().HasOne<User>(c => c.User).WithMany(u => u.Comments).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>().HasOne<User>(l => l.User).WithMany(u => u.Likes).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Like>().HasOne<Photo>(l => l.Photo).WithMany(u => u.Likes).OnDelete(DeleteBehavior.NoAction);

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
