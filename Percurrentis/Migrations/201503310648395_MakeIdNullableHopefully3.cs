namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIdNullableHopefully3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accommodation", "TravelRequestID", c => c.Int());
        }
        
        public override void Down()
        {
        }
    }
}
