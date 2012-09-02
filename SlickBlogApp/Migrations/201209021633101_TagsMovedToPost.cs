namespace SlickBlogApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class TagsMovedToPost : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("TagBlogs", "Tag_TagId", "Tags");
            DropForeignKey("TagBlogs", "Blog_BlogId", "Blogs");
            DropIndex("TagBlogs", new[] { "Tag_TagId" });
            DropIndex("TagBlogs", new[] { "Blog_BlogId" });
            CreateTable(
                "TagPosts",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Post_PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Post_PostId })
                .ForeignKey("Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("Posts", t => t.Post_PostId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Post_PostId);
            
            DropTable("TagBlogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "TagBlogs",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Blog_BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Blog_BlogId });
            
            DropIndex("TagPosts", new[] { "Post_PostId" });
            DropIndex("TagPosts", new[] { "Tag_TagId" });
            DropForeignKey("TagPosts", "Post_PostId", "Posts");
            DropForeignKey("TagPosts", "Tag_TagId", "Tags");
            DropTable("TagPosts");
            CreateIndex("TagBlogs", "Blog_BlogId");
            CreateIndex("TagBlogs", "Tag_TagId");
            AddForeignKey("TagBlogs", "Blog_BlogId", "Blogs", "BlogId", cascadeDelete: true);
            AddForeignKey("TagBlogs", "Tag_TagId", "Tags", "TagId", cascadeDelete: true);
        }
    }
}
