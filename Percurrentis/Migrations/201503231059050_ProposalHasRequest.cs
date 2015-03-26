namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProposalHasRequest : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TravelProposal", "TravelRequestID");
            AddForeignKey("dbo.TravelProposal", "TravelRequestID", "dbo.TravelRequest", "Id");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelProposal", "TravelRequestID", "dbo.TravelRequest");
            DropIndex("dbo.TravelProposal", new[] { "TravelRequestID" });
        }
    }
}
