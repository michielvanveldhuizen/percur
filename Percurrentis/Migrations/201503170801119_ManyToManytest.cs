namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManytest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RequestTraveller", "TravelRequestID", "dbo.TravelRequest");
            DropIndex("dbo.RequestTraveller", new[] { "TravelRequestID" });
            CreateTable(
                "dbo.TravelRequest_Traveller",
                c => new
                    {
                        RequestTravellerId = c.Int(name: "RequestTraveller.Id", nullable: false),
                        TravelRequestId = c.Int(name: "TravelRequest.Id", nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTravellerId, t.TravelRequestId })
                .ForeignKey("dbo.RequestTraveller", t => t.RequestTravellerId, cascadeDelete: true)
                .ForeignKey("dbo.TravelRequest", t => t.TravelRequestId, cascadeDelete: true)
                .Index(t => t.RequestTravellerId)
                .Index(t => t.TravelRequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelRequest_Traveller", "TravelRequest.Id", "dbo.TravelRequest");
            DropForeignKey("dbo.TravelRequest_Traveller", "RequestTraveller.Id", "dbo.RequestTraveller");
            DropIndex("dbo.TravelRequest_Traveller", new[] { "TravelRequest.Id" });
            DropIndex("dbo.TravelRequest_Traveller", new[] { "RequestTraveller.Id" });
            DropTable("dbo.TravelRequest_Traveller");
            CreateIndex("dbo.RequestTraveller", "TravelRequestID");
            AddForeignKey("dbo.RequestTraveller", "TravelRequestID", "dbo.TravelRequest", "Id");
        }
    }
}
