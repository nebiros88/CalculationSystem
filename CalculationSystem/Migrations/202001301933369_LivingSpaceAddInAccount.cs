namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LivingSpaceAddInAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "LivingSpace", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "LivingSpace");
        }
    }
}
