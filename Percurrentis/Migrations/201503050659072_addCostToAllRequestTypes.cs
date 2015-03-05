namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCostToAllRequestTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accommodation", "Cost", c => c.Double());
            AddColumn("dbo.EuroTunnelRequest", "Cost", c => c.Double());
            AddColumn("dbo.RentalCarRequest", "Cost", c => c.Double());
            AddColumn("dbo.TaxiRequest", "Cost", c => c.Double());
            DropColumn("dbo.Accommodation", "DailyRate");
            DropColumn("dbo.Accommodation", "AdditionalFees");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accommodation", "AdditionalFees", c => c.Double());
            AddColumn("dbo.Accommodation", "DailyRate", c => c.Double());
            DropColumn("dbo.TaxiRequest", "Cost");
            DropColumn("dbo.RentalCarRequest", "Cost");
            DropColumn("dbo.EuroTunnelRequest", "Cost");
            DropColumn("dbo.Accommodation", "Cost");
        }
    }
}
