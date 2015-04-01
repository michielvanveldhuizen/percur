namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdNullableHopefully2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequest_Id" });
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequest_Id", newName: "TravelRequestID");
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int(nullable: true));
            CreateIndex("dbo.Accommodation", "TravelRequestID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequestID" });
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int());
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequestID", newName: "TravelRequest_Id");
            CreateIndex("dbo.Accommodation", "TravelRequest_Id");
        }
    }
}
