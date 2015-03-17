namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oneToMany_ManyToOneTest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelRequest_Traveller", "RequestTraveller.Id", "dbo.RequestTraveller");
            DropForeignKey("dbo.TravelRequest_Traveller", "TravelRequest.Id", "dbo.TravelRequest");
            DropIndex("dbo.TravelRequest_Traveller", new[] { "RequestTraveller.Id" });
            DropIndex("dbo.TravelRequest_Traveller", new[] { "TravelRequest.Id" });
            CreateTable(
                "dbo.TravelRequest_RequestTraveller",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestTravellerID = c.Int(nullable: false),
                        TravelRequestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestTraveller", t => t.RequestTravellerID)
                .ForeignKey("dbo.TravelRequest", t => t.TravelRequestID)
                .Index(t => t.RequestTravellerID)
                .Index(t => t.TravelRequestID);
            
            DropTable("dbo.TravelRequest_Traveller");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TravelRequest_Traveller",
                c => new
                    {
                        RequestTravellerId = c.Int(name: "RequestTraveller.Id", nullable: false),
                        TravelRequestId = c.Int(name: "TravelRequest.Id", nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTravellerId, t.TravelRequestId });
            
            DropForeignKey("dbo.TravelRequest_RequestTraveller", "TravelRequestID", "dbo.TravelRequest");
            DropForeignKey("dbo.TravelRequest_RequestTraveller", "RequestTravellerID", "dbo.RequestTraveller");
            DropIndex("dbo.TravelRequest_RequestTraveller", new[] { "TravelRequestID" });
            DropIndex("dbo.TravelRequest_RequestTraveller", new[] { "RequestTravellerID" });
            DropTable("dbo.TravelRequest_RequestTraveller");
            CreateIndex("dbo.TravelRequest_Traveller", "TravelRequest.Id");
            CreateIndex("dbo.TravelRequest_Traveller", "RequestTraveller.Id");
            AddForeignKey("dbo.TravelRequest_Traveller", "TravelRequest.Id", "dbo.TravelRequest", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TravelRequest_Traveller", "RequestTraveller.Id", "dbo.RequestTraveller", "Id", cascadeDelete: true);
        }
    }
}
