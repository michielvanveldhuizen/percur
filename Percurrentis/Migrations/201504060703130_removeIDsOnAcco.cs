namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeIDsOnAcco : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequestID" });
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposalID", newName: "TravelProposal_Id");
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequestID", newName: "TravelRequest_Id");
            AlterColumn("dbo.Accommodation", "TravelRequest_Id", c => c.Int());
            AlterColumn("dbo.Accommodation", "TravelProposal_Id", c => c.Int());
            CreateIndex("dbo.Accommodation", "TravelProposal_Id");
            CreateIndex("dbo.Accommodation", "TravelRequest_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelRequest_Id" });
            DropIndex("dbo.Accommodation", new[] { "TravelProposal_Id" });
            AlterColumn("dbo.Accommodation", "TravelProposal_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Accommodation", "TravelRequest_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Accommodation", name: "TravelRequest_Id", newName: "TravelRequestID");
            RenameColumn(table: "dbo.Accommodation", name: "TravelProposal_Id", newName: "TravelProposalID");
            CreateIndex("dbo.Accommodation", "TravelProposalID");
            CreateIndex("dbo.Accommodation", "TravelRequestID");
        }
    }
}
