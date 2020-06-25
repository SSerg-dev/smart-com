namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SubBrand_and_SubBrand_code_into_Products : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "SubBrand", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "SubBrand_code", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "SubBrand_code");
            DropColumn("dbo.Product", "SubBrand");
        }
    }
}
