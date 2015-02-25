namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TRAremovedHashNextIDApprovalRoleIdADDEDcooapproved : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TravelRequestApproval", name: "ApprovalRoleID", newName: "ApprovalRole_Id");
            RenameColumn(table: "dbo.TravelRequestApproval", name: "NextID", newName: "Next_Id");
            RenameIndex(table: "dbo.TravelRequestApproval", name: "IX_ApprovalRoleID", newName: "IX_ApprovalRole_Id");
            RenameIndex(table: "dbo.TravelRequestApproval", name: "IX_NextID", newName: "IX_Next_Id");
            AddColumn("dbo.TravelRequestApproval", "COOApproved", c => c.Int());
            DropColumn("dbo.TravelRequestApproval", "Hash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelRequestApproval", "Hash", c => c.String(nullable: false));
            DropColumn("dbo.TravelRequestApproval", "COOApproved");
            RenameIndex(table: "dbo.TravelRequestApproval", name: "IX_Next_Id", newName: "IX_NextID");
            RenameIndex(table: "dbo.TravelRequestApproval", name: "IX_ApprovalRole_Id", newName: "IX_ApprovalRoleID");
            RenameColumn(table: "dbo.TravelRequestApproval", name: "Next_Id", newName: "NextID");
            RenameColumn(table: "dbo.TravelRequestApproval", name: "ApprovalRole_Id", newName: "ApprovalRoleID");
        }
    }
}
