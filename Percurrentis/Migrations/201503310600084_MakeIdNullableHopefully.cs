namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdNullableHopefully : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int(nullable: false));
        }
    }
}
