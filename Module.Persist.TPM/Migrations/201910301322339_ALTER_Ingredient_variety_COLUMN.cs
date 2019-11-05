namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALTER_Ingredient_variety_COLUMN : DbMigration
    {
        public override void Up()
        {
            Sql("IF EXISTS(SELECT* FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND XTYPE = 'U') ALTER TABLE[MARS_UNIVERSAL_PETCARE_MATERIALS] ALTER COLUMN[Ingredient_variety] [nvarchar] (40) NULL");
        }
        
        public override void Down()
        {
            Sql("IF EXISTS(SELECT* FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND XTYPE = 'U') ALTER TABLE[MARS_UNIVERSAL_PETCARE_MATERIALS] ALTER COLUMN[Ingredient_variety] [nvarchar] (20) NULL");
        }
    }
}
