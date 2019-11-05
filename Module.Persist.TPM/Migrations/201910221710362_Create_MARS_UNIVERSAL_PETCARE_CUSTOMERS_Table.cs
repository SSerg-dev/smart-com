namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_MARS_UNIVERSAL_PETCARE_CUSTOMERS_Table : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_CUSTOMERS' AND XTYPE = 'U')" +
                "CREATE TABLE [MARS_UNIVERSAL_PETCARE_CUSTOMERS](                                                        " +
                "	[0CUST_SALES] [nvarchar](40) NULL,                                                                   " +
                "	[0SALESORG] [nvarchar](40) NULL,                                                                     " +
                "	[0DISTR_CHAN] [nvarchar](40) NULL,                                                                   " +
                "	[0CUST_SALES___T] [nvarchar](120) NULL,                                                              " +
                "	[ZCUSTHG01] [nvarchar](40) NULL,                                                                     " +
                "	[ZCUSTHG01___T] [nvarchar](120) NULL,                                                                " +
                "	[ZCUSTHG02] [nvarchar](40) NULL,                                                                     " +
                "	[ZCUSTHG02___T] [nvarchar](120) NULL,                                                                " +
                "	[ZCUSTHG03] [nvarchar](40) NULL,                                                                     " +
                "	[ZCUSTHG03___T] [nvarchar](120) NULL,                                                                " +
                "	[ZCUSTHG04] [nvarchar](40) NULL,                                                                     " +
                "	[ZCUSTHG04___T] [nvarchar](120) NULL,                                                                " +
                "	[Active_From] [datetime] NULL,                                                                       " +
                "	[Active_Till] [datetime] NULL,                                                                       " +
                "	[G_H_ParentID] [nvarchar](20) NULL,                                                                  " +
                "	[G_H_level] [nvarchar](20) NULL,                                                                     " +
                "	[SoldToPoint] [nvarchar](20) NULL,                                                                   " +
                "	[SP_Description] [nvarchar](120) NULL,                                                               " +
                ")                                                                                                       "
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'MARS_UNIVERSAL_PETCARE_CUSTOMERS' AND XTYPE = 'U')" +
                "DROP TABLE [MARS_UNIVERSAL_PETCARE_CUSTOMERS]"
            );
        }
    }
}
