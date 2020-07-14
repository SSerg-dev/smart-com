namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discount_ToDouble_MechanicType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MechanicType", "Unique_MechanicType");
            AlterColumn("dbo.MechanicType", "Discount", c => c.Double());
            CreateIndex("dbo.MechanicType", new[] { "DeletedDate", "Name", "Discount", "ClientTreeId" }, unique: true, name: "Unique_MechanicType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.MechanicType", "Unique_MechanicType");
            AlterColumn("dbo.MechanicType", "Discount", c => c.Int());
            CreateIndex("dbo.MechanicType", new[] { "DeletedDate", "Name", "Discount", "ClientTreeId" }, unique: true, name: "Unique_MechanicType");
        }
    }
}
