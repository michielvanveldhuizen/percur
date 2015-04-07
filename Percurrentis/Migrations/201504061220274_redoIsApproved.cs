namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redoIsApproved : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TravelProposal", "IsApproved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelProposal", "IsApproved", c => c.Int(nullable: false));
        }
    }
}
