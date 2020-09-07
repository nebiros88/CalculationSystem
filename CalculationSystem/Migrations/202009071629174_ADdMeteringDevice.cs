namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdMeteringDevice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeteringDevices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: false),
                        Readings = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeteringDevices", "Id", "dbo.Houses");
            DropIndex("dbo.MeteringDevices", new[] { "Id" });
            DropTable("dbo.MeteringDevices");
        }
    }
}
