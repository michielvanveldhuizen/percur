namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accommodationsProposalLink : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EuroTunnelRequest", "TravelProposal_Id", "dbo.TravelProposal");
            DropForeignKey("dbo.FerryRequest", "TravelProposal_Id", "dbo.TravelProposal");
            DropForeignKey("dbo.FlightRequest", "TravelProposal_Id", "dbo.TravelProposal");
            DropForeignKey("dbo.RentalCarRequest", "TravelProposal_Id", "dbo.TravelProposal");
            DropForeignKey("dbo.TaxiRequest", "TravelProposal_Id", "dbo.TravelProposal");
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelProposal_Id" });
            DropIndex("dbo.FerryRequest", new[] { "TravelProposal_Id" });
            DropIndex("dbo.FlightRequest", new[] { "TravelProposal_Id" });
            DropIndex("dbo.RentalCarRequest", new[] { "TravelProposal_Id" });
            DropIndex("dbo.TaxiRequest", new[] { "TravelProposal_Id" });
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposal_Id", newName: "TravelProposalID");
            RenameIndex(table: "dbo.Accommodation", name: "IX_TravelProposal_Id", newName: "IX_TravelProposalID");
            DropColumn("dbo.EuroTunnelRequest", "TravelProposal_Id");
            DropColumn("dbo.FerryRequest", "TravelProposal_Id");
            DropColumn("dbo.FlightRequest", "TravelProposal_Id");
            DropColumn("dbo.RentalCarRequest", "TravelProposal_Id");
            DropColumn("dbo.TaxiRequest", "TravelProposal_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaxiRequest", "TravelProposal_Id", c => c.Int());
            AddColumn("dbo.RentalCarRequest", "TravelProposal_Id", c => c.Int());
            AddColumn("dbo.FlightRequest", "TravelProposal_Id", c => c.Int());
            AddColumn("dbo.FerryRequest", "TravelProposal_Id", c => c.Int());
            AddColumn("dbo.EuroTunnelRequest", "TravelProposal_Id", c => c.Int());
            RenameIndex(table: "dbo.Accommodation", name: "IX_TravelProposalID", newName: "IX_TravelProposal_Id");
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposalID", newName: "TravelProposal_Id");
            CreateIndex("dbo.TaxiRequest", "TravelProposal_Id");
            CreateIndex("dbo.RentalCarRequest", "TravelProposal_Id");
            CreateIndex("dbo.FlightRequest", "TravelProposal_Id");
            CreateIndex("dbo.FerryRequest", "TravelProposal_Id");
            CreateIndex("dbo.EuroTunnelRequest", "TravelProposal_Id");
            AddForeignKey("dbo.TaxiRequest", "TravelProposal_Id", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.RentalCarRequest", "TravelProposal_Id", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.FlightRequest", "TravelProposal_Id", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.FerryRequest", "TravelProposal_Id", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.EuroTunnelRequest", "TravelProposal_Id", "dbo.TravelProposal", "Id");
        }
    }
}
