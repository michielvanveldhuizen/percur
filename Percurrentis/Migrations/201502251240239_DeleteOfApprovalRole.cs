namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteOfApprovalRole : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelRequestApproval", "ApprovalRole_Id", "dbo.ApprovalRole");
            DropForeignKey("dbo.TravelRequestApproval", "Next_Id", "dbo.TravelRequestApproval");
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.TravelRequestApproval", "Next_Id", "dbo.TravelRequestApproval", "Id");
            AddForeignKey("dbo.TravelRequestApproval", "ApprovalRole_Id", "dbo.ApprovalRole", "Id");
        }
    }
}
