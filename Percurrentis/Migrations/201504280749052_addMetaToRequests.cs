namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMetaToRequests : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Accommodation", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.Accommodation", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.Accommodation", "UpdatedBy", c => c.Int());
            AddColumn("dbo.Accommodation", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.Accommodation", "DeletedBy", c => c.Int());
            AddColumn("dbo.Accommodation", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.EuroTunnelRequest", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.EuroTunnelRequest", "UpdatedBy", c => c.Int());
            AddColumn("dbo.EuroTunnelRequest", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.EuroTunnelRequest", "DeletedBy", c => c.Int());
            AddColumn("dbo.EuroTunnelRequest", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FerryRequest", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.FerryRequest", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.FerryRequest", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.FerryRequest", "UpdatedBy", c => c.Int());
            AddColumn("dbo.FerryRequest", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.FerryRequest", "DeletedBy", c => c.Int());
            AddColumn("dbo.FerryRequest", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FlightRequest", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.FlightRequest", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.FlightRequest", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.FlightRequest", "UpdatedBy", c => c.Int());
            AddColumn("dbo.FlightRequest", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.FlightRequest", "DeletedBy", c => c.Int());
            AddColumn("dbo.FlightRequest", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalCarRequest", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RentalCarRequest", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.RentalCarRequest", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.RentalCarRequest", "UpdatedBy", c => c.Int());
            AddColumn("dbo.RentalCarRequest", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.RentalCarRequest", "DeletedBy", c => c.Int());
            AddColumn("dbo.RentalCarRequest", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaxiRequest", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TaxiRequest", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.TaxiRequest", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.TaxiRequest", "UpdatedBy", c => c.Int());
            AddColumn("dbo.TaxiRequest", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.TaxiRequest", "DeletedBy", c => c.Int());
            AddColumn("dbo.TaxiRequest", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "IsDeleted");
            DropColumn("dbo.TaxiRequest", "DeletedBy");
            DropColumn("dbo.TaxiRequest", "DeletedDate");
            DropColumn("dbo.TaxiRequest", "UpdatedBy");
            DropColumn("dbo.TaxiRequest", "UpdatedDate");
            DropColumn("dbo.TaxiRequest", "CreatedBy");
            DropColumn("dbo.TaxiRequest", "CreatedDate");
            DropColumn("dbo.RentalCarRequest", "IsDeleted");
            DropColumn("dbo.RentalCarRequest", "DeletedBy");
            DropColumn("dbo.RentalCarRequest", "DeletedDate");
            DropColumn("dbo.RentalCarRequest", "UpdatedBy");
            DropColumn("dbo.RentalCarRequest", "UpdatedDate");
            DropColumn("dbo.RentalCarRequest", "CreatedBy");
            DropColumn("dbo.RentalCarRequest", "CreatedDate");
            DropColumn("dbo.FlightRequest", "IsDeleted");
            DropColumn("dbo.FlightRequest", "DeletedBy");
            DropColumn("dbo.FlightRequest", "DeletedDate");
            DropColumn("dbo.FlightRequest", "UpdatedBy");
            DropColumn("dbo.FlightRequest", "UpdatedDate");
            DropColumn("dbo.FlightRequest", "CreatedBy");
            DropColumn("dbo.FlightRequest", "CreatedDate");
            DropColumn("dbo.FerryRequest", "IsDeleted");
            DropColumn("dbo.FerryRequest", "DeletedBy");
            DropColumn("dbo.FerryRequest", "DeletedDate");
            DropColumn("dbo.FerryRequest", "UpdatedBy");
            DropColumn("dbo.FerryRequest", "UpdatedDate");
            DropColumn("dbo.FerryRequest", "CreatedBy");
            DropColumn("dbo.FerryRequest", "CreatedDate");
            DropColumn("dbo.EuroTunnelRequest", "IsDeleted");
            DropColumn("dbo.EuroTunnelRequest", "DeletedBy");
            DropColumn("dbo.EuroTunnelRequest", "DeletedDate");
            DropColumn("dbo.EuroTunnelRequest", "UpdatedBy");
            DropColumn("dbo.EuroTunnelRequest", "UpdatedDate");
            DropColumn("dbo.EuroTunnelRequest", "CreatedBy");
            DropColumn("dbo.EuroTunnelRequest", "CreatedDate");
            DropColumn("dbo.Accommodation", "IsDeleted");
            DropColumn("dbo.Accommodation", "DeletedBy");
            DropColumn("dbo.Accommodation", "DeletedDate");
            DropColumn("dbo.Accommodation", "UpdatedBy");
            DropColumn("dbo.Accommodation", "UpdatedDate");
            DropColumn("dbo.Accommodation", "CreatedBy");
            DropColumn("dbo.Accommodation", "CreatedDate");
        }
    }
}
