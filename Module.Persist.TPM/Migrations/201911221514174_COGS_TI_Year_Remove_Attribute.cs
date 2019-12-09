namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class COGS_TI_Year_Remove_Attribute : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.COGS", "Year", c => c.Int(nullable: false));
            AlterColumn("dbo.TradeInvestment", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeInvestment", "Year", c => c.Int(nullable: false));
            AlterColumn("dbo.COGS", "Year", c => c.Int(nullable: false));
        }
    }
}
