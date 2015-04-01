namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reapplyForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FerryRequest", new[] { "TravelRequest_Id" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelRequest_Id" });
            DropIndex("dbo.TaxiRequest", new[] { "TravelRequest_Id" });
            RenameColumn(table: "dbo.FerryRequest", name: "TravelProposal_Id", newName: "TravelProposalID");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.EuroTunnelRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.TaxiRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameIndex(table: "dbo.FerryRequest", name: "IX_TravelProposal_Id", newName: "IX_TravelProposalID");
            AddColumn("dbo.EuroTunnelRequest", "TravelProposalID", c => c.Int(nullable: true));
            AddColumn("dbo.RentalCarRequest", "TravelProposalID", c => c.Int(nullable: true));
            AddColumn("dbo.TaxiRequest", "TravelProposalID", c => c.Int(nullable: true));
            AlterColumn("dbo.FerryRequest", "TravelRequestID", c => c.Int(nullable: true));
            AlterColumn("dbo.EuroTunnelRequest", "TravelRequestID", c => c.Int(nullable: true));
            AlterColumn("dbo.TaxiRequest", "TravelRequestID", c => c.Int(nullable: true));
            CreateIndex("dbo.EuroTunnelRequest", "TravelRequestID");
            CreateIndex("dbo.EuroTunnelRequest", "TravelProposalID");
            CreateIndex("dbo.FerryRequest", "TravelRequestID");
            CreateIndex("dbo.RentalCarRequest", "TravelProposalID");
            CreateIndex("dbo.TaxiRequest", "TravelRequestID");
            CreateIndex("dbo.TaxiRequest", "TravelProposalID");
            AddForeignKey("dbo.EuroTunnelRequest", "TravelProposalID", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.RentalCarRequest", "TravelProposalID", "dbo.TravelProposal", "Id");
            AddForeignKey("dbo.TaxiRequest", "TravelProposalID", "dbo.TravelProposal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaxiRequest", "TravelProposalID", "dbo.TravelProposal");
            DropForeignKey("dbo.RentalCarRequest", "TravelProposalID", "dbo.TravelProposal");
            DropForeignKey("dbo.EuroTunnelRequest", "TravelProposalID", "dbo.TravelProposal");
            DropIndex("dbo.TaxiRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.TaxiRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.RentalCarRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.FerryRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelProposalID" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelRequestID" });
            AlterColumn("dbo.TaxiRequest", "TravelRequestID", c => c.Int());
            AlterColumn("dbo.EuroTunnelRequest", "TravelRequestID", c => c.Int());
            AlterColumn("dbo.FerryRequest", "TravelRequestID", c => c.Int());
            DropColumn("dbo.TaxiRequest", "TravelProposalID");
            DropColumn("dbo.RentalCarRequest", "TravelProposalID");
            DropColumn("dbo.EuroTunnelRequest", "TravelProposalID");
            RenameIndex(table: "dbo.FerryRequest", name: "IX_TravelProposalID", newName: "IX_TravelProposal_Id");
            RenameColumn(table: "dbo.TaxiRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.EuroTunnelRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelProposalID", newName: "TravelProposal_Id");
            CreateIndex("dbo.TaxiRequest", "TravelRequest_Id");
            CreateIndex("dbo.EuroTunnelRequest", "TravelRequest_Id");
            CreateIndex("dbo.FerryRequest", "TravelRequest_Id");
        }
    }
}
