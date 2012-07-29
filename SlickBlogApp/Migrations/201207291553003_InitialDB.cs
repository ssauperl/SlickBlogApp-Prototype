namespace SlickBlogApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "UserInfo",
                c => new
                    {
                        UserInfoId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        UserGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserInfoId);
            
            CreateTable(
                "Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Address = c.String(nullable: false, maxLength: 30),
                        Owner_UserInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlogId)
                .ForeignKey("UserInfo", t => t.Owner_UserInfoId)
                .Index(t => t.Owner_UserInfoId);
            
            CreateTable(
                "Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        Content = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                        Blog_BlogId = c.Int(),
                        Author_UserInfoId = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("Blogs", t => t.Blog_BlogId)
                .ForeignKey("UserInfo", t => t.Author_UserInfoId)
                .Index(t => t.Blog_BlogId)
                .Index(t => t.Author_UserInfoId);
            
            CreateTable(
                "Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        Post_PostId = c.Int(),
                        Author_UserInfoId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("Posts", t => t.Post_PostId)
                .ForeignKey("UserInfo", t => t.Author_UserInfoId)
                .Index(t => t.Post_PostId)
                .Index(t => t.Author_UserInfoId);
            
            CreateTable(
                "TagBlogs",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Blog_BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Blog_BlogId })
                .ForeignKey("Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("Blogs", t => t.Blog_BlogId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Blog_BlogId);
            
            CreateTable(
                "BlogUserInfo",
                c => new
                    {
                        BlogId = c.Int(nullable: false),
                        UserInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BlogId, t.UserInfoId })
                .ForeignKey("Blogs", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("UserInfo", t => t.UserInfoId, cascadeDelete: true)
                .Index(t => t.BlogId)
                .Index(t => t.UserInfoId);
            
        }
        
        public override void Down()
        {
            DropIndex("BlogUserInfo", new[] { "UserInfoId" });
            DropIndex("BlogUserInfo", new[] { "BlogId" });
            DropIndex("TagBlogs", new[] { "Blog_BlogId" });
            DropIndex("TagBlogs", new[] { "Tag_TagId" });
            DropIndex("Comments", new[] { "Author_UserInfoId" });
            DropIndex("Comments", new[] { "Post_PostId" });
            DropIndex("Posts", new[] { "Author_UserInfoId" });
            DropIndex("Posts", new[] { "Blog_BlogId" });
            DropIndex("Blogs", new[] { "Owner_UserInfoId" });
            DropForeignKey("BlogUserInfo", "UserInfoId", "UserInfo");
            DropForeignKey("BlogUserInfo", "BlogId", "Blogs");
            DropForeignKey("TagBlogs", "Blog_BlogId", "Blogs");
            DropForeignKey("TagBlogs", "Tag_TagId", "Tags");
            DropForeignKey("Comments", "Author_UserInfoId", "UserInfo");
            DropForeignKey("Comments", "Post_PostId", "Posts");
            DropForeignKey("Posts", "Author_UserInfoId", "UserInfo");
            DropForeignKey("Posts", "Blog_BlogId", "Blogs");
            DropForeignKey("Blogs", "Owner_UserInfoId", "UserInfo");
            DropTable("BlogUserInfo");
            DropTable("TagBlogs");
            DropTable("Comments");
            DropTable("Posts");
            DropTable("Tags");
            DropTable("Blogs");
            DropTable("UserInfo");
        }
    }
}
