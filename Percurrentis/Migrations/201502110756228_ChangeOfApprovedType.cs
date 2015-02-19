namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOfApprovedType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TravelRequest", "IsApproved", c => c.Int(nullable: false));
            AlterColumn("dbo.TravelRequestApproval", "HasApproved", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TravelRequestApproval", "HasApproved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.TravelRequest", "IsApproved", c => c.Boolean(nullable: false));
        }
    }
}
