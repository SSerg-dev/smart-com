namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Release_01_To_OtherTypesPromo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.COGS", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.TradeInvestment", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeInvestment", "Year");
            DropColumn("dbo.COGS", "Year");
        }
    }
}
