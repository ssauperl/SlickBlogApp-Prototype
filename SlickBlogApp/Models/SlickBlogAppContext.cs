using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SlickBlogApp.Models
{
    public class SlickBlogAppContext : DbContext
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            //modelBuilder.Entity<Blog>().HasRequired(b => b.Owner).WithMany(ui => ui.Blogs).HasForeignKey(b => b.BlogId).WillCascadeOnDelete();
            modelBuilder.Entity<Blog>().HasMany(b => b.Authors).WithMany(ui => ui.AuthBlogs).Map(mp =>
            {
                mp.ToTable("BlogUserInfo");
                mp.MapLeftKey("BlogId");
                mp.MapRightKey("UserInfoId");
            });
            modelBuilder.Entity<Blog>().HasRequired(b => b.Owner).WithMany(ui => ui.Blogs).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Blog>().HasMany(b => b.Authors).WithMany(ui => ui.AuthBlogs);
            base.OnModelCreating(modelBuilder);
        }
    }
     
}