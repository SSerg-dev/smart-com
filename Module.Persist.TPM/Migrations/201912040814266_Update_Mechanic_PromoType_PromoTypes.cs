namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Mechanic_PromoType_PromoTypes : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Mechanic", name: "PromoTypeId", newName: "PromoTypesId");
            RenameIndex(table: "dbo.Mechanic", name: "IX_PromoTypeId", newName: "IX_PromoTypesId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Mechanic", name: "IX_PromoTypesId", newName: "IX_PromoTypeId");
            RenameColumn(table: "dbo.Mechanic", name: "PromoTypesId", newName: "PromoTypeId");
        }
    }
}
