namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddTrIdforRental : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RentalCarRequest", new[] { "TravelRequest_Id" });
            RenameColumn(table: "dbo.RentalCarRequest", name: "TravelRequest_Id", newName: "TravelRequestID");
            AlterColumn("dbo.RentalCarRequest", "TravelRequestID", c => c.Int());
            CreateIndex("dbo.RentalCarRequest", "TravelRequestID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RentalCarRequest", new[] { "TravelRequestID" });
            AlterColumn("dbo.RentalCarRequest", "TravelRequestID", c => c.Int());
            RenameColumn(table: "dbo.RentalCarRequest", name: "TravelRequestID", newName: "TravelRequest_Id");
            CreateIndex("dbo.RentalCarRequest", "TravelRequest_Id");
        }
    }
}
