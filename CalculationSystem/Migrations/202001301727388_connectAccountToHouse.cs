namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connectAccountToHouse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "ApartmentNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Accounts", "HouseId", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "HouseId");
            AddForeignKey("dbo.Accounts", "HouseId", "dbo.Houses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "HouseId", "dbo.Houses");
            DropIndex("dbo.Accounts", new[] { "HouseId" });
            DropColumn("dbo.Accounts", "HouseId");
            DropColumn("dbo.Accounts", "ApartmentNumber");
        }
    }
}
