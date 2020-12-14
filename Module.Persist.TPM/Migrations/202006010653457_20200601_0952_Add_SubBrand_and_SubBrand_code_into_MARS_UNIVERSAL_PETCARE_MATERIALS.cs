namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200601_0952_Add_SubBrand_and_SubBrand_code_into_MARS_UNIVERSAL_PETCARE_MATERIALS : DbMigration
    {
        public override void Up()
        {
            Sql("alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [SubBrand] [nvarchar](80) NULL " +
                "alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [SubBrand_code] [nvarchar](80) NULL ");
        }
        
        public override void Down()
        {
            Sql("alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] drop column [SubBrand] " +
                "alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] drop column [SubBrand_code] ");
        }
    }
}
