namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TICOGS_Change_Percents_To_Float : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.COGS", "LVSpercent", c => c.Single(nullable: false));
            AlterColumn("dbo.TradeInvestment", "SizePercent", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TradeInvestment", "SizePercent", c => c.Short(nullable: false));
            AlterColumn("dbo.COGS", "LVSpercent", c => c.Short(nullable: false));
        }
    }
}
