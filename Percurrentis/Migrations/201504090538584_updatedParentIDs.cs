namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedParentIDs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalCarRequest", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RentalCarRequest", "ParentID");
        }
    }
}
