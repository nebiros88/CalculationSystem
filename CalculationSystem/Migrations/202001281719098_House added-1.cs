namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Houseadded1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Street = c.String(),
                        HouseNumber = c.Int(nullable: false),
                        HeatingStandart = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Houses");
        }
    }
}
