namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_YEAR_END_ESTIMATE_FDM_Table : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'YEAR_END_ESTIMATE_FDM' AND XTYPE = 'U')  " +
                "CREATE TABLE [YEAR_END_ESTIMATE_FDM](                                                          " +
                "        [YEAR] [nvarchar](50) NULL,                                                            " +
                "		[BRAND_SEG_TECH_CODE] [nvarchar](100) NULL,                                             " +
                "		[G_HIERARCHY_ID] [nvarchar](100) NULL,                                                  " +
                "		[DMR_PLAN_LSV] [float] NULL,                                                            " +
                "		[YTD_LSV] [float] NULL,                                                                 " +
                "		[YEE_LSV] [float] NULL                                                                  " +
                ")                                                                                              "
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'YEAR_END_ESTIMATE_FDM' AND XTYPE = 'U')" +
                "DROP TABLE [YEAR_END_ESTIMATE_FDM]"
            );
        }
    }
}
