namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAccrualValueColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accruals", "Value", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accruals", "Value", c => c.Int(nullable: false));
        }
    }
}
