namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20200601_1201_Add_BrandsegTech_and_BrandsegTech_code_into_MARS_UNIVERSAL_PETCARE_MATERIALS : DbMigration
    {
        public override void Up()
        {
            Sql("alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [BrandsegTechsub] [nvarchar](80) NULL " +
                "alter table [MARS_UNIVERSAL_PETCARE_MATERIALS] add [BrandsegTechsub_code] [nvarchar](80) NULL ");
        }

        public override void Down()
        {
            Sql("ALTER TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [BrandsegTechsub] " +
                "ALTER TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS] DROP COLUMN [BrandsegTechsub_code] ");
        }
    }
}
