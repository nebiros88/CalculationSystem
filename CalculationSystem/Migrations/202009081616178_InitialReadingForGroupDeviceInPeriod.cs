namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialReadingForGroupDeviceInPeriod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InitialHouseDeviceReadingInPeriods",
                c => new
                    {
                        HouseId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        Readings = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.HouseId, t.PeriodId })
                .ForeignKey("dbo.Houses", t => t.HouseId, cascadeDelete: true)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.HouseId)
                .Index(t => t.PeriodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InitialHouseDeviceReadingInPeriods", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.InitialHouseDeviceReadingInPeriods", "HouseId", "dbo.Houses");
            DropIndex("dbo.InitialHouseDeviceReadingInPeriods", new[] { "PeriodId" });
            DropIndex("dbo.InitialHouseDeviceReadingInPeriods", new[] { "HouseId" });
            DropTable("dbo.InitialHouseDeviceReadingInPeriods");
        }
    }
}
