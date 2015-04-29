namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class implementBaseRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.FerryRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlightRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalCarRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaxiRequest", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "Active");
            DropColumn("dbo.RentalCarRequest", "Active");
            DropColumn("dbo.FlightRequest", "Active");
            DropColumn("dbo.FerryRequest", "Active");
            DropColumn("dbo.EuroTunnelRequest", "Active");
            DropColumn("dbo.Accommodation", "Active");
        }
    }
}
