namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelProposal_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TravelProposal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsApproved = c.Boolean(nullable: false),
                        TravelRequestID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.Int(),
                        DeletedDate = c.DateTime(),
                        DeletedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Accommodation", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.Accommodation", "TravelProposalID");
            AddForeignKey("dbo.Accommodation", "TravelProposalID", "dbo.TravelProposal", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accommodation", "TravelProposalID", "dbo.TravelProposal");
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            DropColumn("dbo.Accommodation", "TravelProposalID");
            DropTable("dbo.TravelProposal");
        }
    }
}
