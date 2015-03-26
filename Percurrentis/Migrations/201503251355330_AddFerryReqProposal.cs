namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFerryReqProposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EuroTunnelRequest", "TravelProposalID", c => c.Int());
            AddColumn("dbo.FerryRequest", "TravelProposalID", c => c.Int());
            AddColumn("dbo.TaxiRequest", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.FerryRequest", "TravelProposalID");
            AddForeignKey("dbo.FerryRequest", "TravelProposalID", "dbo.TravelProposal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FerryRequest", "TravelProposalID", "dbo.TravelProposal");
            DropIndex("dbo.FerryRequest", new[] { "TravelProposalID" });
            DropColumn("dbo.TaxiRequest", "TravelProposalID");
            DropColumn("dbo.FerryRequest", "TravelProposalID");
            DropColumn("dbo.EuroTunnelRequest", "TravelProposalID");
        }
    }
}
