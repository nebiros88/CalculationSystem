namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnerwasaddedtoAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Owner", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "Owner");
        }
    }
}
