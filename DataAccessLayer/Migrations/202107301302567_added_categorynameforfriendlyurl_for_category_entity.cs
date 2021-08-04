namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_categorynameforfriendlyurl_for_category_entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryNameForFriendlyUrl", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "CategoryNameForFriendlyUrl");
        }
    }
}
