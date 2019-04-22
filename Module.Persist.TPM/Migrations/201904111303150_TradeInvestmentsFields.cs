namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeInvestmentsFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeInvestment", "MarcCalcROI", c => c.Boolean(nullable: false));
            AddColumn("dbo.TradeInvestment", "MarcCalcBudgets", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeInvestment", "MarcCalcBudgets");
            DropColumn("dbo.TradeInvestment", "MarcCalcROI");
        }
    }
}
