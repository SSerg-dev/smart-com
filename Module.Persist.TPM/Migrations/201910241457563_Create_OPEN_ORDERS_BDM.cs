namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_OPEN_ORDERS_BDM : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'OPEN_ORDERS_BDM' AND XTYPE = 'U')" +
                "CREATE TABLE [OPEN_ORDERS_BDM](                                                        " +
                "	[LSV] [numeric](30, 8) NULL,                                                        " +
                "	[DELIVERY_NO] [nvarchar](40) NULL,                                                  " +
                "	[NET_WT_AP] [numeric](30, 8) NULL,                                                  " +
                "	[MATERIAL_CODE] [nvarchar](40) NULL,                                                " +
                "	[GROSS_WGT] [numeric](30, 8) NULL,                                                  " +
                "	[PGI_DATE] [datetime] NULL,                                                         " +
                "	[PLANT] [nvarchar](40) NULL,                                                        " +
                "	[RECORD_DATETIME] [datetime] NULL,                                                  " +
                "	[SALES_ORDER_DATE] [datetime] NULL,                                                 " +
                "	[SALES_ORDER_NO] [nvarchar](40) NULL,                                               " +
                "	[SHIP_TO_CODE] [nvarchar](40) NULL,                                                 " +
                "	[SOLD_TO_CODE] [nvarchar](40) NULL                                                  " +
                ")                                                                                      "
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'OPEN_ORDERS_BDM' AND XTYPE = 'U')" +
                "DROP TABLE [OPEN_ORDERS_BDM]"
            );
        }
    }
}
