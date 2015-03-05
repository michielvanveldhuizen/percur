namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableCosts : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FerryRequest", "Cost", c => c.Double());
            AlterColumn("dbo.FlightRequest", "Cost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FlightRequest", "Cost", c => c.Double(nullable: false));
            AlterColumn("dbo.FerryRequest", "Cost", c => c.Double(nullable: false));
        }
    }
}
