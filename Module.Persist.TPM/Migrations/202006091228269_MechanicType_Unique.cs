namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MechanicType_Unique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MechanicType", new[] { "ClientTreeId" });
            CreateIndex("dbo.MechanicType", new[] { "DeletedDate", "Name", "Discount", "ClientTreeId" }, unique: true, name: "Unique_MechanicType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MechanicType", "Unique_MechanicType");
            CreateIndex("dbo.MechanicType", "ClientTreeId");
        }
    }
}
