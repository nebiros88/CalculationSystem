namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccruals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accruals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accruals", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.Accruals", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Accruals", new[] { "AccountId" });
            DropIndex("dbo.Accruals", new[] { "PeriodId" });
            DropTable("dbo.Accruals");
        }
    }
}
