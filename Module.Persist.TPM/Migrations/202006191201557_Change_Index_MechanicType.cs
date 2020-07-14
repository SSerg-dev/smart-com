namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Index_MechanicType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MechanicType", "Unique_MechanicType");
            CreateIndex("dbo.MechanicType", new[] { "DeletedDate", "Name", "ClientTreeId" }, unique: true, name: "Unique_MechanicType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MechanicType", "Unique_MechanicType");
            CreateIndex("dbo.MechanicType", new[] { "DeletedDate", "Name", "Discount", "ClientTreeId" }, unique: true, name: "Unique_MechanicType");
        }
    }
}
