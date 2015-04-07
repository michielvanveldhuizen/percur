namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovalIntOnProposal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TravelProposal", "IsApproved", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TravelProposal", "IsApproved", c => c.Boolean(nullable: false));
        }
    }
}
