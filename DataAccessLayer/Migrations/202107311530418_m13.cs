namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageStatus",
                c => new
                    {
                        MessageStatusId = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsOpened = c.Boolean(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageStatusId)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            AddColumn("dbo.Messages", "SenderUsername", c => c.String(maxLength: 50));
            AddColumn("dbo.Messages", "ReceiverUsername", c => c.String(maxLength: 50));
            DropColumn("dbo.Messages", "SenderMail");
            DropColumn("dbo.Messages", "ReceiverMail");
            DropColumn("dbo.Messages", "IsOpened");
            DropColumn("dbo.Messages", "IsDraft");
            DropColumn("dbo.Messages", "IsArchived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "IsArchived", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "IsDraft", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "IsOpened", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "ReceiverMail", c => c.String(maxLength: 50));
            AddColumn("dbo.Messages", "SenderMail", c => c.String(maxLength: 50));
            DropForeignKey("dbo.MessageStatus", "MessageId", "dbo.Messages");
            DropIndex("dbo.MessageStatus", new[] { "MessageId" });
            DropColumn("dbo.Messages", "ReceiverUsername");
            DropColumn("dbo.Messages", "SenderUsername");
            DropTable("dbo.MessageStatus");
        }
    }
}
