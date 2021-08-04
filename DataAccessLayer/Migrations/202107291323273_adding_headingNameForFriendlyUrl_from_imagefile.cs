namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_headingNameForFriendlyUrl_from_imagefile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageFiles", "ImageNameForFriendlyUrl", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageFiles", "ImageNameForFriendlyUrl");
        }
    }
}
