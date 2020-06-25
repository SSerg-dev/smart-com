namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_ROLLING_VOLUMES_FDM : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ROLLING_VOLUMES_FDM' AND XTYPE = 'U')    " +
                "   CREATE TABLE [dbo].[ROLLING_VOLUMES_FDM](				                                    " +
                "   	[ZREP] [nvarchar](100) NULL,				                                            " +
                "   	[DMDGROUP] [nvarchar](max) NULL,				                                        " +
                "   	[WeekStartDate] [datetime] NULL,				                                        " +
                "   	[PlanProductIncrementalQty] [float] NULL,			                                    " +
                "   	[ActualsQty] [float] NULL,				                                                " +
                "   	[OpenOrdersQty] [float] NULL,				                                            " +
                "   	[BaselineQty] [float] NULL,				                                                " +
                "   	[ActualIncrementalQty] [float] NULL,				                                    " +
                "   	[PreviousRollingVolumesQty] [float] NULL,			                                    " +
                "   	[PromoDifferenceQty] [float] NULL,             		                                    " +
                "   	[RollingVolumesQty] [float] NULL  			                                            " +
                "   ) ON [PRIMARY]                                                                              "
                );
        }
        
        public override void Down()
        {
            Sql("IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ROLLING_VOLUMES_FDM' AND XTYPE = 'U') " +
                "DROP TABLE [ROLLING_VOLUMES_FDM]"
            );
        }
    }
}
