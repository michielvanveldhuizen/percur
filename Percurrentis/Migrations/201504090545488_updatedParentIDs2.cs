namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedParentIDs2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EuroTunnelRequest", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EuroTunnelRequest", "ParentID");
        }
    }
}
