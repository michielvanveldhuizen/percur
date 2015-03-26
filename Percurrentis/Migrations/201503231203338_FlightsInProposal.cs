namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightsInProposal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FlightRequest", new[] { "TravelProposalID" });
            AlterColumn("dbo.FlightRequest", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.FlightRequest", "TravelProposalID");

        }
        
        public override void Down()
        {
            DropIndex("dbo.FlightRequest", new[] { "TravelProposalID" });
            AlterColumn("dbo.FlightRequest", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.FlightRequest", "TravelProposalID");
        }
    }
}
