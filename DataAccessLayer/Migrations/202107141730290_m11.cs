namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m11 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contents", "ContentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contents", "ContentStatus", c => c.Boolean(nullable: false));
        }
    }
}
