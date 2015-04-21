namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redoTrIdforRental : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RentalCarRequest", new[] { "TravelRequestID" });
            RenameColumn(table: "dbo.RentalCarRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            AlterColumn("dbo.RentalCarRequest", "TravelRequest_Id", c => c.Int());
            CreateIndex("dbo.RentalCarRequest", "TravelRequest_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RentalCarRequest", new[] { "TravelRequest_Id" });
            AlterColumn("dbo.RentalCarRequest", "TravelRequest_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.RentalCarRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            CreateIndex("dbo.RentalCarRequest", "TravelRequestID");
        }
    }
}
