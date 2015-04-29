namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCopyOf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "CopyOf", c => c.Int());
            AddColumn("dbo.EuroTunnelRequest", "CopyOf", c => c.Int());
            AddColumn("dbo.FerryRequest", "CopyOf", c => c.Int());
            AddColumn("dbo.FlightRequest", "CopyOf", c => c.Int());
            AddColumn("dbo.RentalCarRequest", "CopyOf", c => c.Int());
            AddColumn("dbo.TaxiRequest", "CopyOf", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "CopyOf");
            DropColumn("dbo.RentalCarRequest", "CopyOf");
            DropColumn("dbo.FlightRequest", "CopyOf");
            DropColumn("dbo.FerryRequest", "CopyOf");
            DropColumn("dbo.EuroTunnelRequest", "CopyOf");
            DropColumn("dbo.Accommodation", "CopyOf");
        }
    }
}
