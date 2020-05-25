namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_PRICELIST_FDM : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'PRICELIST_FDM' AND XTYPE = 'U')" +
                "	CREATE TABLE [PRICELIST_FDM](                                                     " +
                "		[G_HIERARCHY_ID] [nvarchar](100) NULL,                                        " +
                "		[ZREP] [nvarchar](100) NULL,                                                  " +
                "		[PRICE] [float] NULL,                                                         " +
                "		[START_DATE] [datetime] NULL,                                                 " +
                "		[FINISH_DATE] [datetime] NULL,                                                " +
                "		[UNIT_OF_MEASURE] [nvarchar](100) NULL,                                       " +
                "		[CURRENCY] [nvarchar](100) NULL                                               " +
                "   )                                                                                 "
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'PRICELIST_FDM' AND XTYPE = 'U')" +
                "DROP TABLE [PRICELIST_FDM]"
            );
        }
    }
}
