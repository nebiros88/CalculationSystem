namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Price_Service_Account_Relationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rate = c.Double(nullable: false),
                        ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId);
            
            CreateTable(
                "dbo.ServiceAccounts",
                c => new
                    {
                        Service_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Service_Id, t.Account_Id })
                .ForeignKey("dbo.Services", t => t.Service_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Service_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prices", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.ServiceAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.ServiceAccounts", "Service_Id", "dbo.Services");
            DropIndex("dbo.ServiceAccounts", new[] { "Account_Id" });
            DropIndex("dbo.ServiceAccounts", new[] { "Service_Id" });
            DropIndex("dbo.Prices", new[] { "ServiceId" });
            DropTable("dbo.ServiceAccounts");
            DropTable("dbo.Prices");
            DropTable("dbo.Services");
        }
    }
}
