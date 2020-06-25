namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200529_1427_Add_SubBrand_and_SubBrand_code_into_Technology : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Technology", "SubBrand", c => c.String(maxLength: 255));
            AddColumn("dbo.Technology", "SubBrand_code", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Technology", "SubBrand_code");
            DropColumn("dbo.Technology", "SubBrand");
        }
    }
}
