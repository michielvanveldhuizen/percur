namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedByToString : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Accommodation DROP CONSTRAINT DF__Accommoda__Creat__4F47C5E3");
            AlterColumn("dbo.Accommodation", "CreatedBy", c => c.String());
            AlterColumn("dbo.Accommodation", "UpdatedBy", c => c.String());
            AlterColumn("dbo.Accommodation", "DeletedBy", c => c.String());
            AlterColumn("dbo.TravelProposal", "CreatedBy", c => c.String());
            AlterColumn("dbo.TravelProposal", "UpdatedBy", c => c.String());
            AlterColumn("dbo.TravelProposal", "DeletedBy", c => c.String());
            Sql("ALTER TABLE dbo.EuroTunnelRequest DROP CONSTRAINT DF__EuroTunne__Creat__5224328E");
            AlterColumn("dbo.EuroTunnelRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.EuroTunnelRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.EuroTunnelRequest", "DeletedBy", c => c.String());
            AlterColumn("dbo.TravelRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.TravelRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.TravelRequest", "DeletedBy", c => c.String());
            Sql("ALTER TABLE dbo.FerryRequest DROP CONSTRAINT DF__FerryRequ__Creat__55009F39");
            AlterColumn("dbo.FerryRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.FerryRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.FerryRequest", "DeletedBy", c => c.String());
            Sql("ALTER TABLE dbo.FlightRequest DROP CONSTRAINT DF__FlightReq__Creat__57DD0BE4");
            AlterColumn("dbo.FlightRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.FlightRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.FlightRequest", "DeletedBy", c => c.String());
            Sql("ALTER TABLE dbo.RentalCarRequest DROP CONSTRAINT DF__RentalCar__Creat__5AB9788F");
            AlterColumn("dbo.RentalCarRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.RentalCarRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.RentalCarRequest", "DeletedBy", c => c.String());
            Sql("ALTER TABLE dbo.TaxiRequest DROP CONSTRAINT DF__TaxiReque__Creat__5D95E53A");
            AlterColumn("dbo.TaxiRequest", "CreatedBy", c => c.String());
            AlterColumn("dbo.TaxiRequest", "UpdatedBy", c => c.String());
            AlterColumn("dbo.TaxiRequest", "DeletedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaxiRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.TaxiRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.TaxiRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.RentalCarRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.RentalCarRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.RentalCarRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.FlightRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.FlightRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.FlightRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.FerryRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.FerryRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.FerryRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TravelRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.TravelRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.TravelRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.EuroTunnelRequest", "DeletedBy", c => c.Int());
            AlterColumn("dbo.EuroTunnelRequest", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.EuroTunnelRequest", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.TravelProposal", "DeletedBy", c => c.Int());
            AlterColumn("dbo.TravelProposal", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.TravelProposal", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Accommodation", "DeletedBy", c => c.Int());
            AlterColumn("dbo.Accommodation", "UpdatedBy", c => c.Int());
            AlterColumn("dbo.Accommodation", "CreatedBy", c => c.Int(nullable: false));
        }
    }
}
