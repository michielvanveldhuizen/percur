namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TRAcooApprovedIsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TravelRequestApproval", "COOApproved", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TravelRequestApproval", "COOApproved", c => c.Int());
        }
    }
}
