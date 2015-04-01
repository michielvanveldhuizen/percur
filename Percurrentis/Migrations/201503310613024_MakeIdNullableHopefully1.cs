namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdNullableHopefully1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequestID" });
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequestID", newName: "TravelRequest_Id");
            AlterColumn("dbo.Accommodation", "TravelRequest_Id", c => c.Int());
            CreateIndex("dbo.Accommodation", "TravelRequest_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequest_Id" });
            AlterColumn("dbo.Accommodation", "TravelRequest_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequest_Id", newName: "TravelRequestID");
            CreateIndex("dbo.Accommodation", "TravelRequestID");
        }
    }
}
