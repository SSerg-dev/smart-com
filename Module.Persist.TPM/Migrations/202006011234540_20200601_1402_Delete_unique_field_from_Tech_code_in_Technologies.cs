namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200601_1402_Delete_unique_field_from_Tech_code_in_Technologies : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Technology", "Unique_Name");
            AlterColumn("dbo.BrandTech", "BrandsegTechsub", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BrandTech", "BrandsegTechsub", c => c.String());
            CreateIndex("dbo.Technology", new[] { "Name", "Disabled", "DeletedDate", "Tech_code" }, unique: true, name: "Unique_Name");
        }
    }
}
