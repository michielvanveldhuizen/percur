namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rebuildAccommodations : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelProposal_Id" });
            DropIndex("dbo.Accommodation", new[] { "TravelRequest_Id" });
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposal_Id", newName: "TravelProposalID");
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequest_Id", newName: "TravelRequestID");
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int(nullable: true));
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int(nullable: true));
            CreateIndex("dbo.Accommodation", "TravelRequestID");
            CreateIndex("dbo.Accommodation", "TravelProposalID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            DropIndex("dbo.Accommodation", new[] { "TravelRequestID" });
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int());
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int());
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequestID", newName: "TravelRequest_Id");
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposalID", newName: "TravelProposal_Id");
            CreateIndex("dbo.Accommodation", "TravelRequest_Id");
            CreateIndex("dbo.Accommodation", "TravelProposal_Id");
        }
    }
}
