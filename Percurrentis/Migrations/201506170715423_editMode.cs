namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editMode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelProposal", "inEditMode", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelProposal", "inEditMode");
        }
    }
}
