namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_BrandSegTechSub_In_MARS_UNIVERSAL_PETCARE_MATERIALS : DbMigration
    {
        public override void Up()
        {
            Sql("EXEC sys.sp_rename @objname = N'[MARS_UNIVERSAL_PETCARE_MATERIALS].[BrandsegTechsub]', @newname = 'BrandSegTechSub', @objtype = 'COLUMN' " +
                "EXEC sys.sp_rename @objname = N'[MARS_UNIVERSAL_PETCARE_MATERIALS].[BrandsegTechsub_code]', @newname = 'BrandSegTechSub_code', @objtype = 'COLUMN' ");
        }
        
        public override void Down()
        {
            Sql("EXEC sys.sp_rename @objname = N'[MARS_UNIVERSAL_PETCARE_MATERIALS].[BrandSegTechSub]', @newname = 'BrandsegTechsub', @objtype = 'COLUMN' " +
                "EXEC sys.sp_rename @objname = N'[MARS_UNIVERSAL_PETCARE_MATERIALS].[BrandSegTechSub_code]', @newname = 'BrandsegTechsub_code', @objtype = 'COLUMN' ");
        }
    }
}
