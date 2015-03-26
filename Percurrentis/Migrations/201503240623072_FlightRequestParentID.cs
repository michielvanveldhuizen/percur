namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlightRequestParentID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlightRequest", "ParentID", c => c.Int());

        }
        
        public override void Down()
        {
            DropColumn("dbo.FlightRequest", "ParentID");
        }
    }
}
