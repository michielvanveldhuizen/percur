namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProposalHasApproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelProposal", "TravelRequestApprovalID", c => c.Int());
            CreateIndex("dbo.TravelProposal", "TravelRequestApprovalID");
            AddForeignKey("dbo.TravelProposal", "TravelRequestApprovalID", "dbo.TravelRequestApproval", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelProposal", "TravelRequestApprovalID", "dbo.TravelRequestApproval");
            DropIndex("dbo.TravelProposal", new[] { "TravelRequestApprovalID" });
            DropColumn("dbo.TravelProposal", "TravelRequestApprovalID");
        }
    }
}
