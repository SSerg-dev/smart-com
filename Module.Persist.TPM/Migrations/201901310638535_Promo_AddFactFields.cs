namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_AddFactFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "FactUplift", c => c.Int());
            AddColumn("dbo.Promo", "FactIncrementalLsv", c => c.Int());
            AddColumn("dbo.Promo", "FactTotalPromoLsv", c => c.Int());
            AddColumn("dbo.Promo", "FactPostPromoEffect", c => c.Int());
            AddColumn("dbo.Promo", "FactRoi", c => c.Int());
            AddColumn("dbo.Promo", "FactIncrementalNsv", c => c.Int());
            AddColumn("dbo.Promo", "FactTotalPromoNsv", c => c.Int());
            AddColumn("dbo.Promo", "FactIncrementalMac", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "FactIncrementalMac");
            DropColumn("dbo.Promo", "FactTotalPromoNsv");
            DropColumn("dbo.Promo", "FactIncrementalNsv");
            DropColumn("dbo.Promo", "FactRoi");
            DropColumn("dbo.Promo", "FactPostPromoEffect");
            DropColumn("dbo.Promo", "FactTotalPromoLsv");
            DropColumn("dbo.Promo", "FactIncrementalLsv");
            DropColumn("dbo.Promo", "FactUplift");
        }
    }
}
