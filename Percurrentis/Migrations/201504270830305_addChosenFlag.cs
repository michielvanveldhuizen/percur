namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChosenFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "Chosen", c => c.Boolean(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "Chosen", c => c.Boolean(nullable: false));
            AddColumn("dbo.FerryRequest", "Chosen", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlightRequest", "Chosen", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalCarRequest", "Chosen", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaxiRequest", "Chosen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "Chosen");
            DropColumn("dbo.RentalCarRequest", "Chosen");
            DropColumn("dbo.FlightRequest", "Chosen");
            DropColumn("dbo.FerryRequest", "Chosen");
            DropColumn("dbo.EuroTunnelRequest", "Chosen");
            DropColumn("dbo.Accommodation", "Chosen");
        }
    }
}
