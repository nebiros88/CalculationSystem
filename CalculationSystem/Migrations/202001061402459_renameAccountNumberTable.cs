namespace CalculationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameAccountNumberTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccountNumbers", newName: "Accounts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Accounts", newName: "AccountNumbers");
        }
    }
}
