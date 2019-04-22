namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromoIncident_Change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "PromoIntId", c => c.Int());
            AddColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanic", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanic", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "OldMechanicInStore", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "NewMechanicInStore", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "IsCreate", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromoDemandChangeIncident", "IsDelete", c => c.Boolean(nullable: false));
            DropColumn("dbo.PromoDemandChangeIncident", "PromoId");
            DropColumn("dbo.PromoDemandChangeIncident", "OldMechanicType");
            DropColumn("dbo.PromoDemandChangeIncident", "NewMechanicType");
            DropColumn("dbo.PromoDemandChangeIncident", "OldMechanicInStoreType");
            DropColumn("dbo.PromoDemandChangeIncident", "NewMechanicInStoreType");
            DropColumn("dbo.PromoDemandChangeIncident", "IsNew");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PromoDemandChangeIncident", "IsNew", c => c.Boolean(nullable: false));
            AddColumn("dbo.PromoDemandChangeIncident", "NewMechanicInStoreType", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "OldMechanicInStoreType", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "NewMechanicType", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "OldMechanicType", c => c.String(maxLength: 20));
            AddColumn("dbo.PromoDemandChangeIncident", "PromoId", c => c.Guid(nullable: false));
            DropColumn("dbo.PromoDemandChangeIncident", "IsDelete");
            DropColumn("dbo.PromoDemandChangeIncident", "IsCreate");
            DropColumn("dbo.PromoDemandChangeIncident", "NewMechanicInStore");
            DropColumn("dbo.PromoDemandChangeIncident", "OldMechanicInStore");
            DropColumn("dbo.PromoDemandChangeIncident", "NewMarsMechanic");
            DropColumn("dbo.PromoDemandChangeIncident", "OldMarsMechanic");
            DropColumn("dbo.PromoDemandChangeIncident", "PromoIntId");
        }
    }
}
