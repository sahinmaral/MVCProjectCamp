namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abouts", "AboutHeaderForFriendlyUrl", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Abouts", "AboutHeaderForFriendlyUrl");
        }
    }
}
