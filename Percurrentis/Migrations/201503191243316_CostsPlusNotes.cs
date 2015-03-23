namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostsPlusNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "CostSecondary", c => c.Double());
            AddColumn("dbo.Accommodation", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.Accommodation", "Note", c => c.String());
            AddColumn("dbo.FlightRequest", "CostSecondary", c => c.Double());
            AddColumn("dbo.FlightRequest", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.FlightRequest", "Note", c => c.String());
            AddColumn("dbo.EuroTunnelRequest", "CostSecondary", c => c.Double());
            AddColumn("dbo.EuroTunnelRequest", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.EuroTunnelRequest", "Note", c => c.String());
            AddColumn("dbo.FerryRequest", "CostSecondary", c => c.Double());
            AddColumn("dbo.FerryRequest", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.FerryRequest", "Note", c => c.String());
            AddColumn("dbo.RentalCarRequest", "CostSecondary", c => c.Double());
            AddColumn("dbo.RentalCarRequest", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.RentalCarRequest", "Note", c => c.String());
            AddColumn("dbo.TaxiRequest", "CostSecondary", c => c.Double());
            AddColumn("dbo.TaxiRequest", "SecondaryCurrency", c => c.String());
            AddColumn("dbo.TaxiRequest", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "Note");
            DropColumn("dbo.TaxiRequest", "SecondaryCurrency");
            DropColumn("dbo.TaxiRequest", "CostSecondary");
            DropColumn("dbo.RentalCarRequest", "Note");
            DropColumn("dbo.RentalCarRequest", "SecondaryCurrency");
            DropColumn("dbo.RentalCarRequest", "CostSecondary");
            DropColumn("dbo.FerryRequest", "Note");
            DropColumn("dbo.FerryRequest", "SecondaryCurrency");
            DropColumn("dbo.FerryRequest", "CostSecondary");
            DropColumn("dbo.EuroTunnelRequest", "Note");
            DropColumn("dbo.EuroTunnelRequest", "SecondaryCurrency");
            DropColumn("dbo.EuroTunnelRequest", "CostSecondary");
            DropColumn("dbo.FlightRequest", "Note");
            DropColumn("dbo.FlightRequest", "SecondaryCurrency");
            DropColumn("dbo.FlightRequest", "CostSecondary");
            DropColumn("dbo.Accommodation", "Note");
            DropColumn("dbo.Accommodation", "SecondaryCurrency");
            DropColumn("dbo.Accommodation", "CostSecondary");
        }
    }
}
