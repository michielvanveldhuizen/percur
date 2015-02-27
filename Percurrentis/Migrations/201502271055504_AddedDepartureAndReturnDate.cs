namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDepartureAndReturnDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelRequest", "DepartureDate", c => c.DateTime());
            AddColumn("dbo.TravelRequest", "ReturnDate", c => c.DateTime());
            AddColumn("dbo.TravelRequest", "IsItinerary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelRequest", "IsItinerary");
            DropColumn("dbo.TravelRequest", "ReturnDate");
            DropColumn("dbo.TravelRequest", "DepartureDate");
        }
    }
}
