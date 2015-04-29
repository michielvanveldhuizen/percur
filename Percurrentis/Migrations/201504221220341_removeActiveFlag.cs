namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeActiveFlag : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Accommodation", "Active");
            DropColumn("dbo.EuroTunnelRequest", "Active");
            DropColumn("dbo.FerryRequest", "Active");
            DropColumn("dbo.FlightRequest", "Active");
            DropColumn("dbo.RentalCarRequest", "Active");
            DropColumn("dbo.TaxiRequest", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaxiRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalCarRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlightRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.FerryRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Accommodation", "Active", c => c.Boolean(nullable: false));
        }
    }
}
