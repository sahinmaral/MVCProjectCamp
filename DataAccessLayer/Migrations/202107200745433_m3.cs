namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Abouts", "AboutHeader", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Abouts", "AboutHeader", c => c.String(maxLength: 1000));
        }
    }
}
