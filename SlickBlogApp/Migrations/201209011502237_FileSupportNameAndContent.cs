namespace SlickBlogApp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FileSupportNameAndContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("Posts", "FileName", c => c.String());
            AddColumn("Posts", "FileContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Posts", "FileContentType");
            DropColumn("Posts", "FileName");
        }
    }
}
