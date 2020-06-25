namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Delete_UniqueIndex_from_Name_in_Technologies : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Technology", "Unique_Tech");
            CreateIndex("dbo.Technology", new[] { "Tech_code", "SubBrand_code", "Disabled", "DeletedDate", "Name" }, unique: true, name: "Unique_Tech");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Technology", "Unique_Tech");
            CreateIndex("dbo.Technology", new[] { "Name", "Tech_code", "SubBrand_code", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Tech");
        }
    }
}
