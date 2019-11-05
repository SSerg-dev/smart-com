namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_MARS_UNIVERSAL_PETCARE_MATERIALS_Table : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND XTYPE = 'U')" +
                "CREATE TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS](                                                        " +
                "	[VKORG] [nvarchar](40) NULL,                                                                         " +
                "	[MATNR] [nvarchar](40) NULL,                                                                         " +
                "	[VMSTD] [datetime] NULL,                                                                             " +
                "	[0CREATEDON] [nvarchar](20) NULL,                                                                    " +
                "	[0DIVISION] [nvarchar](20) NULL,                                                                     " +
                "	[0DIVISION___T] [nvarchar](40) NULL,                                                                 " +
                "	[MATERIAL] [nvarchar](40) NULL,                                                                      " +
                "	[SKU] [nvarchar](120) NULL,                                                                          " +
                "	[0MATL_TYPE___T] [nvarchar](120) NULL,                                                               " +
                "	[0UNIT_OF_WT] [nvarchar](20) NULL,                                                                   " +
                "	[GGROSS_WT] [numeric](30, 8) NULL,                                                                   " +
                "	[GNET_WT] [numeric](30, 8) NULL,                                                                     " +
                "	[Brand] [nvarchar](40) NULL,                                                                         " +
                "	[Segmen] [nvarchar](40) NULL,                                                                        " +
                "	[Technology] [nvarchar](40) NULL,                                                                    " +
                "	[BrandTech] [nvarchar](80) NULL,                                                                     " +
                "	[BrandTech_code] [nvarchar](20) NULL,                                                                " +
                "	[Brand_code] [nvarchar](20) NULL,                                                                    " +
                "	[Tech_code] [nvarchar](20) NULL,                                                                     " +
                "	[ZREP] [nvarchar](40) NULL,                                                                          " +
                "	[EAN_Case] [numeric](30, 8) NULL,                                                                    " +
                "	[EAN_PC] [numeric](30, 8) NULL,                                                                      " +
                "	[Brand_Flag_abbr] [nvarchar](20) NULL,                                                               " +
                "	[Brand_Flag] [nvarchar](20) NULL,                                                                    " +
                "	[Submark_Flag] [nvarchar](40) NULL,                                                                  " +
                "	[Ingredient_variety] [nvarchar](20) NULL,                                                            " +
                "	[Product_Category] [nvarchar](20) NULL,                                                              " +
                "	[Product_Type] [nvarchar](40) NULL,                                                                  " +
                "	[Supply_Segment] [nvarchar](40) NULL,                                                                " +
                "	[Functional_variety] [nvarchar](40) NULL,                                                            " +
                "	[Size] [nvarchar](20) NULL,                                                                          " +
                "	[Brand_essence] [nvarchar](40) NULL,                                                                 " +
                "	[Pack_Type] [nvarchar](40) NULL,                                                                     " +
                "	[Traded_unit_format] [nvarchar](40) NULL,                                                            " +
                "	[Consumer_pack_format] [nvarchar](40) NULL,                                                          " +
                "	[UOM_PC2Case] [nvarchar](20) NULL,                                                                   " +
                "	[Segmen_code] [nvarchar](20) NULL,                                                                   " +
                "	[BrandsegTech_code] [nvarchar](40) NULL,                                                             " +
                "	[count] [int] NULL,                                                                                  " +
                "	[Brandsegtech] [nvarchar](80) NULL                                                                   " +
                ")                                                                                                       " 
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND XTYPE = 'U')" +
                "DROP TABLE [MARS_UNIVERSAL_PETCARE_MATERIALS]"
            );
        }
    }
}
