namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class killTreqIdKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FlightRequest", "TravelRequestID", "dbo.TravelRequest");
            DropIndex("dbo.FlightRequest", new[] { "TravelRequestID" });
            
        }
        
        public override void Down()
        {
            CreateIndex("dbo.FlightRequest", "TravelRequestID");
            AddForeignKey("dbo.FlightRequest", "TravelRequestID", "dbo.TravelRequest", "Id");
        }
    }
}
