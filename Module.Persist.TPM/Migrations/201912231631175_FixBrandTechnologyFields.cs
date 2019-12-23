namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBrandTechnologyFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Brand", "Unique_Name");
            DropIndex("dbo.Technology", "Unique_Name");
            AlterColumn("dbo.Brand", "Brand_code", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Brand", "Segmen_code", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.Technology", "Tech_code", c => c.String(maxLength: 255, unicode: false));
            AlterColumn("dbo.BrandTech", "BrandTech_code", c => c.String());
            AlterColumn("dbo.Product", "BrandTech_code", c => c.String(maxLength: 255));
            AlterColumn("dbo.Product", "BrandsegTech_code", c => c.String(maxLength: 255));
            CreateIndex("dbo.Brand", new[] { "Name", "Disabled", "DeletedDate", "Brand_code", "Segmen_code" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.Technology", new[] { "Name", "Disabled", "DeletedDate", "Tech_code" }, unique: true, name: "Unique_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Technology", "Unique_Name");
            DropIndex("dbo.Brand", "Unique_Name");
            AlterColumn("dbo.Product", "BrandsegTech_code", c => c.String(maxLength: 255));
            AlterColumn("dbo.Product", "BrandTech_code", c => c.String(maxLength: 255));
            AlterColumn("dbo.BrandTech", "BrandTech_code", c => c.String());
            AlterColumn("dbo.Technology", "Tech_code", c => c.String());
            AlterColumn("dbo.Brand", "Segmen_code", c => c.String());
            AlterColumn("dbo.Brand", "Brand_code", c => c.String());
            CreateIndex("dbo.Technology", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
            CreateIndex("dbo.Brand", new[] { "Name", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Name");
        }
    }
}
