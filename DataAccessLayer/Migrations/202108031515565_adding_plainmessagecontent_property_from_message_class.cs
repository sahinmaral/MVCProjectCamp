namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_plainmessagecontent_property_from_message_class : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "PlainMessageContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "PlainMessageContent");
        }
    }
}
