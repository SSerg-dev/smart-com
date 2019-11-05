namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoDemandChange_Notification_FieldsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "PromoStatus", c => c.String());
            DropColumn("dbo.PromoDemandChangeIncident", "OldPromoStatus");
            DropColumn("dbo.PromoDemandChangeIncident", "NewPromoStatus");
            DropColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanic");
            DropColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanic");
            DropColumn("dbo.PromoDemandChangeIncident", "OldOutletCount");
            DropColumn("dbo.PromoDemandChangeIncident", "NewOutletCount");
            DropColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanicDiscount");
            DropColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanicDiscount");
            DropColumn("dbo.PromoDemandChangeIncident", "OldStartDate");
            DropColumn("dbo.PromoDemandChangeIncident", "NewStartDate");
            DropColumn("dbo.PromoDemandChangeIncident", "OldEndDate");
            DropColumn("dbo.PromoDemandChangeIncident", "NewEndDate");
            DropColumn("dbo.PromoDemandChangeIncident", "OldDispatchesEnd");
            DropColumn("dbo.PromoDemandChangeIncident", "NewDispatchesEnd");
            DropColumn("dbo.PromoDemandChangeIncident", "OldXSite");
            DropColumn("dbo.PromoDemandChangeIncident", "NEWXSite");
            DropColumn("dbo.PromoDemandChangeIncident", "OldCatalogue");
            DropColumn("dbo.PromoDemandChangeIncident", "NEWCatalogue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "NEWCatalogue", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "OldCatalogue", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "NEWXSite", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "OldXSite", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "NewDispatchesEnd", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "OldDispatchesEnd", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "NewEndDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "OldEndDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "NewStartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "OldStartDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanicDiscount", c => c.Double());
            AddColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanicDiscount", c => c.Double());
            AddColumn("dbo.PromoDemandChangeIncident", "NewOutletCount", c => c.Int());
            AddColumn("dbo.PromoDemandChangeIncident", "OldOutletCount", c => c.Int());
            AddColumn("dbo.PromoDemandChangeIncident", "NewPlanInstoreMechanic", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "OldPlanInstoreMechanic", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "NewPromoStatus", c => c.String());
            AddColumn("dbo.PromoDemandChangeIncident", "OldPromoStatus", c => c.String());
            DropColumn("dbo.PromoDemandChangeIncident", "PromoStatus");
        }
    }
}
