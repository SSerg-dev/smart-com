namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductZREPUnique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Product", new[] { "ZREP", "Disabled", "DeletedDate" }, unique: true, name: "Unique_ZREP");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Product", "Unique_ZREP");
        }
    }
}
