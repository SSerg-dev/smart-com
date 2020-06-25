namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Unique_to_Technology : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Technology", new[] { "Name", "Tech_code", "SubBrand_code", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Tech");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Technology", "Unique_Tech");
        }
    }
}
