namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeparmentForRequestTraveller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestTraveller", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestTraveller", "Department");
        }
    }
}
