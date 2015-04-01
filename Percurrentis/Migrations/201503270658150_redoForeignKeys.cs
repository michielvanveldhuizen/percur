namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redoForeignKeys : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FerryRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelRequestID" });
            DropIndex("dbo.TaxiRequest", new[] { "TravelRequestID" });
            RenameColumn(table: "dbo.FerryRequest", name: "TravelProposalID", newName: "TravelProposal_Id");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.EuroTunnelRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.TaxiRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameIndex(table: "dbo.FerryRequest", name: "IX_TravelProposalID", newName: "IX_TravelProposal_Id");
            AlterColumn("dbo.FerryRequest", "TravelRequest_Id", c => c.Int());
            AlterColumn("dbo.EuroTunnelRequest", "TravelRequest_Id", c => c.Int());
            AlterColumn("dbo.TaxiRequest", "TravelRequest_Id", c => c.Int());
            CreateIndex("dbo.FerryRequest", "TravelRequest_Id");
            CreateIndex("dbo.EuroTunnelRequest", "TravelRequest_Id");
            CreateIndex("dbo.TaxiRequest", "TravelRequest_Id");
            DropColumn("dbo.EuroTunnelRequest", "TravelProposalID");
            DropColumn("dbo.TaxiRequest", "TravelProposalID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaxiRequest", "TravelProposalID", c => c.Int());
            AddColumn("dbo.EuroTunnelRequest", "TravelProposalID", c => c.Int());
            DropIndex("dbo.TaxiRequest", new[] { "TravelRequest_Id" });
            DropIndex("dbo.EuroTunnelRequest", new[] { "TravelRequest_Id" });
            DropIndex("dbo.FerryRequest", new[] { "TravelRequest_Id" });
            AlterColumn("dbo.TaxiRequest", "TravelRequest_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.EuroTunnelRequest", "TravelRequest_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.FerryRequest", "TravelRequest_Id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.FerryRequest", name: "IX_TravelProposal_Id", newName: "IX_TravelProposalID");
            RenameColumn(table: "dbo.TaxiRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.EuroTunnelRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.FerryRequest", name: "TravelProposal_Id", newName: "TravelProposalID");
            CreateIndex("dbo.TaxiRequest", "TravelRequestID");
            CreateIndex("dbo.EuroTunnelRequest", "TravelRequestID");
            CreateIndex("dbo.FerryRequest", "TravelRequestID");
        }
    }
}
