namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedReasonToProposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelProposal", "Reason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelProposal", "Reason");
        }
    }
}
