namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_Actual_To_PromoProduct : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Actual", newName: "PromoProduct");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PromoProduct", newName: "Actual");
        }
    }
}
