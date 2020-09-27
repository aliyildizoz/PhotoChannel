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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=PhotoChannel; Trusted_Connection=true");
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<Photo>(entity =>
        //    //    {
        //    //        entity.HasMany(p => p.Likes).WithOne(l => l.Photo).HasForeignKey(like => like.PhotoId).OnDelete(DeleteBehavior.Cascade);
        //    //        entity.HasMany(p => p.Comments).WithOne(c => c.Photo).HasForeignKey(comment => comment.PhotoId).OnDelete(DeleteBehavior.Cascade);
        //    //    });
        //    //modelBuilder.Entity<Like>(builder =>
        //    //{
        //    //    builder.HasOne(l => l.Photo).WithMany(photo => photo.Likes).HasForeignKey(like => like.PhotoId).OnDelete(DeleteBehavior.Cascade);
        //    //});
        //}

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
