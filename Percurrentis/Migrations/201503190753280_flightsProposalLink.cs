namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flightsProposalLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlightRequest", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.FlightRequest", "TravelProposalID");
            AddForeignKey("dbo.FlightRequest", "TravelProposalID", "dbo.TravelProposal", "Id");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlightRequest", "TravelProposalID", "dbo.TravelProposal");
            DropIndex("dbo.FlightRequest", new[] { "TravelProposalID" });
            DropColumn("dbo.FlightRequest", "TravelProposalID");
        }
    }
}
