namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightRequestRefactor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlightRequest", "Airline", c => c.String());
            AddColumn("dbo.FlightRequest", "Cost", c => c.Double(nullable: false));
            DropColumn("dbo.FlightRequest", "AirlineID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FlightRequest", "AirlineID", c => c.Int());
            DropColumn("dbo.FlightRequest", "Cost");
            DropColumn("dbo.FlightRequest", "Airline");
        }
    }
}
