namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reenableIsApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelProposal", "IsApproved", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelProposal", "IsApproved");
        }
    }
}
