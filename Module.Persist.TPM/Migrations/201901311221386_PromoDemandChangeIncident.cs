namespace Module.Persist.TPM.Migrations {
    using System.Data.Entity.Migrations;

    public partial class PromoDemandChangeIncident : DbMigration {
        public override void Up() {
            CreateTable(
                "dbo.PromoDemandChangeIncident",
                c => new {
                    Id = c.Guid(nullable: false, identity: true),
                    PromoId = c.Guid(nullable: false),
                    Name = c.String(maxLength: 255),
                    ClientHierarchy = c.String(),
                    BrandTech = c.String(),
                    PromoStatus = c.String(),
                    OldMechanicType = c.String(maxLength: 20),
                    NewMechanicType = c.String(maxLength: 20),
                    OldMechanicInStoreType = c.String(maxLength: 20),
                    NewMechanicInStoreType = c.String(maxLength: 20),
                    OldOutletCount = c.Int(),
                    NewOutletCount = c.Int(),
                    OldMarsMechanicDiscount = c.Int(),
                    NewMarsMechanicDiscount = c.Int(),
                    OldInstoreMechanicDiscount = c.Int(),
                    NewInstoreMechanicDiscount = c.Int(),
                    OldStartDate = c.DateTimeOffset(precision: 7),
                    NewStartDate = c.DateTimeOffset(precision: 7),
                    OldEndDate = c.DateTimeOffset(precision: 7),
                    NewEndDate = c.DateTimeOffset(precision: 7),
                    OldDispatchesStart = c.DateTimeOffset(precision: 7),
                    NewDispatchesStart = c.DateTimeOffset(precision: 7),
                    OldDispatchesEnd = c.DateTimeOffset(precision: 7),
                    NewDispatchesEnd = c.DateTimeOffset(precision: 7),
                    OldPlanUplift = c.Int(),
                    NewPlanUplift = c.Int(),
                    OldPlanIncrementalLsv = c.Int(),
                    NewPlanIncrementalLsv = c.Int(),
                    OldPlanSteel = c.Int(),
                    NewPlanSteel = c.Int(),
                    OldXSite = c.String(),
                    NEWXSite = c.String(),
                    OldCatalogue = c.String(),
                    NEWCatalogue = c.String(),
                    IsNew = c.Boolean(nullable: false),
                    IsProductListChange = c.Boolean(nullable: false),
                    ProcessDate = c.DateTimeOffset(precision: 7),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down() {
            DropTable("dbo.PromoDemandChangeIncident");
        }
    }
}
