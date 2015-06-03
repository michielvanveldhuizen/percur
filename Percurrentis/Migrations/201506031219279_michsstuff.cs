namespace Percurrentis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class michsstuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailError",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        ToEmail = c.String(),
                        TypeOfEmail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RequestTraveller", "PassportNumber", c => c.String());
            AddColumn("dbo.RequestTraveller", "PassportExpiryDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RequestTraveller", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.RequestTraveller", "Gender", c => c.String());
            AddColumn("dbo.RequestTraveller", "Nationality", c => c.String());
            AddColumn("dbo.RequestTraveller", "Note", c => c.String());
            AddColumn("dbo.RequestTraveller", "AddressID", c => c.Int());
            AddColumn("dbo.RequestTraveller", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RequestTraveller", "CreatedBy", c => c.String());
            AddColumn("dbo.RequestTraveller", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.RequestTraveller", "UpdatedBy", c => c.String());
            AddColumn("dbo.RequestTraveller", "DeletedDate", c => c.DateTime());
            AddColumn("dbo.RequestTraveller", "DeletedBy", c => c.String());
            AddColumn("dbo.RequestTraveller", "IsDeleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RequestTraveller", "AddressID");
            AddForeignKey("dbo.RequestTraveller", "AddressID", "dbo.Address", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestTraveller", "AddressID", "dbo.Address");
            DropIndex("dbo.RequestTraveller", new[] { "AddressID" });
            DropColumn("dbo.RequestTraveller", "IsDeleted");
            DropColumn("dbo.RequestTraveller", "DeletedBy");
            DropColumn("dbo.RequestTraveller", "DeletedDate");
            DropColumn("dbo.RequestTraveller", "UpdatedBy");
            DropColumn("dbo.RequestTraveller", "UpdatedDate");
            DropColumn("dbo.RequestTraveller", "CreatedBy");
            DropColumn("dbo.RequestTraveller", "CreatedDate");
            DropColumn("dbo.RequestTraveller", "AddressID");
            DropColumn("dbo.RequestTraveller", "Note");
            DropColumn("dbo.RequestTraveller", "Nationality");
            DropColumn("dbo.RequestTraveller", "Gender");
            DropColumn("dbo.RequestTraveller", "DateOfBirth");
            DropColumn("dbo.RequestTraveller", "PassportExpiryDate");
            DropColumn("dbo.RequestTraveller", "PassportNumber");
            DropTable("dbo.MailError");
        }
    }
}
