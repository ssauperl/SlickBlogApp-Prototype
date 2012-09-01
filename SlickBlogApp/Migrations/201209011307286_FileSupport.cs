namespace SlickBlogApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FileSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("Posts", "File", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("Posts", "File");
        }
    }
}
