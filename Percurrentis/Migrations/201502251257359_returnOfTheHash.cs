namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnOfTheHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelRequestApproval", "Hash", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelRequestApproval", "Hash");
        }
    }
}
