namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blijkbaarDingen : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.FlightRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.RentalCarRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.TaxiRequest", new[] { "TravelProposalID" });
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int());
            AlterColumn("dbo.EuroTunnelRequest", "TravelProposalID", c => c.Int());
            AlterColumn("dbo.FlightRequest", "TravelProposalID", c => c.Int());
            AlterColumn("dbo.RentalCarRequest", "TravelProposalID", c => c.Int());
            AlterColumn("dbo.TaxiRequest", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.Accommodation", "TravelProposalID");
            CreateIndex("dbo.EuroTunnelRequest", "TravelProposalID");
            CreateIndex("dbo.FlightRequest", "TravelProposalID");
            CreateIndex("dbo.RentalCarRequest", "TravelProposalID");
            CreateIndex("dbo.TaxiRequest", "TravelProposalID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TaxiRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.RentalCarRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.FlightRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            AlterColumn("dbo.TaxiRequest", "TravelProposalID", c => c.Int(nullable: false));
            AlterColumn("dbo.RentalCarRequest", "TravelProposalID", c => c.Int(nullable: false));
            AlterColumn("dbo.FlightRequest", "TravelProposalID", c => c.Int(nullable: false));
            AlterColumn("dbo.EuroTunnelRequest", "TravelProposalID", c => c.Int(nullable: false));
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int(nullable: false));
            CreateIndex("dbo.TaxiRequest", "TravelProposalID");
            CreateIndex("dbo.RentalCarRequest", "TravelProposalID");
            CreateIndex("dbo.FlightRequest", "TravelProposalID");
            CreateIndex("dbo.EuroTunnelRequest", "TravelProposalID");
            CreateIndex("dbo.Accommodation", "TravelProposalID");
        }
    }
}
