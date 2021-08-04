namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleting_username_from_contact : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contacts", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Username", c => c.String(maxLength: 50));
        }
    }
}
