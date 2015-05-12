namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProposalFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "IsProposalItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "IsProposalItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.FerryRequest", "IsProposalItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlightRequest", "IsProposalItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalCarRequest", "IsProposalItem", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaxiRequest", "IsProposalItem", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "IsProposalItem");
            DropColumn("dbo.RentalCarRequest", "IsProposalItem");
            DropColumn("dbo.FlightRequest", "IsProposalItem");
            DropColumn("dbo.FerryRequest", "IsProposalItem");
            DropColumn("dbo.EuroTunnelRequest", "IsProposalItem");
            DropColumn("dbo.Accommodation", "IsProposalItem");
        }
    }
}
