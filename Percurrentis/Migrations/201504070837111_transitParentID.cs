namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transitParentID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EuroTunnelRequest", "ParentID", c => c.Int());
            AddColumn("dbo.TaxiRequest", "ParentID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxiRequest", "ParentID");
            DropColumn("dbo.EuroTunnelRequest", "ParentID");
        }
    }
}
