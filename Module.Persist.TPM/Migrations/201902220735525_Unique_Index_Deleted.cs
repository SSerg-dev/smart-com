namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Unique_Index_Deleted : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Brand", new[] { "Name" });
            DropIndex("dbo.Technology", new[] { "Name" });
            DropIndex("dbo.RetailType", new[] { "Name" });
            CreateIndex("dbo.Brand", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.Technology", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.RetailType", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.RetailType", "Unique_Name");
            DropIndex("dbo.Technology", "Unique_Name");
            DropIndex("dbo.Brand", "Unique_Name");
            CreateIndex("dbo.RetailType", "Name", unique: true);
            CreateIndex("dbo.Technology", "Name", unique: true);
            CreateIndex("dbo.Brand", "Name", unique: true);
        }
    }
}
