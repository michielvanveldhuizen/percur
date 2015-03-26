namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apparentlySomethingChanged : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int(nullable: true));
            CreateIndex("dbo.Accommodation", "TravelProposalID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accommodation", new[] { "TravelProposalID" });
            AlterColumn("dbo.Accommodation", "TravelProposalID", c => c.Int());
            CreateIndex("dbo.Accommodation", "TravelProposalID");
        }
    }
}
