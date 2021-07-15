namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abouts", "AboutHeader", c => c.String(maxLength: 1000));
            AddColumn("dbo.Abouts", "AboutText", c => c.String(maxLength: 1000));
            DropColumn("dbo.Abouts", "AboutDetails1");
            DropColumn("dbo.Abouts", "AboutDetails2");
            DropColumn("dbo.Abouts", "AboutImage");
            DropColumn("dbo.Abouts", "AboutImage2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Abouts", "AboutImage2", c => c.String());
            AddColumn("dbo.Abouts", "AboutImage", c => c.String(maxLength: 100));
            AddColumn("dbo.Abouts", "AboutDetails2", c => c.String(maxLength: 1000));
            AddColumn("dbo.Abouts", "AboutDetails1", c => c.String(maxLength: 1000));
            DropColumn("dbo.Abouts", "AboutText");
            DropColumn("dbo.Abouts", "AboutHeader");
        }
    }
}
