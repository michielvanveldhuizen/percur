namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TRAgotnomoreTA : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelRequestApproval", "TravelRequestID", "dbo.TravelRequest");
            DropIndex("dbo.TravelRequestApproval", new[] { "TravelRequestID" });
            DropColumn("dbo.TravelRequestApproval", "TravelRequestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelRequestApproval", "TravelRequestID", c => c.Int());
            CreateIndex("dbo.TravelRequestApproval", "TravelRequestID");
            AddForeignKey("dbo.TravelRequestApproval", "TravelRequestID", "dbo.TravelRequest", "Id");
        }
    }
}
