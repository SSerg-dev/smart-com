namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_ActualInStoreMechanic : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Promo", name: "ActualInStoreMechanicsId", newName: "ActualInStoreMechanicId");
            RenameIndex(table: "dbo.Promo", name: "IX_ActualInStoreMechanicsId", newName: "IX_ActualInStoreMechanicId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Promo", name: "IX_ActualInStoreMechanicId", newName: "IX_ActualInStoreMechanicsId");
            RenameColumn(table: "dbo.Promo", name: "ActualInStoreMechanicId", newName: "ActualInStoreMechanicsId");
        }
    }
}
