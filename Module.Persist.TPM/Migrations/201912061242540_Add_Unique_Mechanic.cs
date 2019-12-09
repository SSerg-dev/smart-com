namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Unique_Mechanic : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Mechanic", new[] { "PromoTypesId" });
            CreateIndex("dbo.Mechanic", new[] { "Disabled", "DeletedDate", "Name", "PromoTypesId" }, unique: true, name: "Unique_Mechanic");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mechanic", "Unique_Mechanic");
            CreateIndex("dbo.Mechanic", "PromoTypesId");
        }
    }
}
