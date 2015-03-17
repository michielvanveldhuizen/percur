namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManytest1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RequestTraveller", "TravelRequestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RequestTraveller", "TravelRequestID", c => c.Int(nullable: false));
        }
    }
}
