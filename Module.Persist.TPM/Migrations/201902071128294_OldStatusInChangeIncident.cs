namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OldStatusInChangeIncident : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "OldPromoStatus", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "NewPromoStatus", c => c.String());
            DropColumn("dbo.PromoDemandChangeIncident", "PromoStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "PromoStatus", c => c.String());
            DropColumn("dbo.PromoDemandChangeIncident", "NewPromoStatus");
            DropColumn("dbo.PromoDemandChangeIncident", "OldPromoStatus");
        }
    }
}
