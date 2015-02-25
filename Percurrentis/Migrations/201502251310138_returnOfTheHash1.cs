namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnOfTheHash1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelRequest", "Hash", c => c.String(nullable: false));
            DropColumn("dbo.TravelRequestApproval", "Hash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelRequestApproval", "Hash", c => c.String(nullable: false));
            DropColumn("dbo.TravelRequest", "Hash");
        }
    }
}
