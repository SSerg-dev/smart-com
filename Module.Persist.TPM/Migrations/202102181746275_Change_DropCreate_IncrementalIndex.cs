namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_DropCreate_IncrementalIndex : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }

        public override void Down()
        {
        }

        private string SqlString =
			@"
				ALTER   PROCEDURE [DefaultSchemaSetting].[DropIncrementalIndex]
				@IsCurrent BIT
				AS
				BEGIN
					IF @IsCurrent = 1
					BEGIN
						IF EXISTS (SELECT * FROM SYS.INDEXES
										WHERE NAME = N'IX_CurrentDayIncremental_References_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'CurrentDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							DROP INDEX [IX_CurrentDayIncremental_References_NONCLUSTERED] ON [DefaultSchemaSetting].[CurrentDayIncremental];
	
						IF EXISTS (SELECT * FROM SYS.INDEXES
										WHERE NAME = N'IX_CurrentDayIncremental_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'CurrentDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							DROP INDEX [IX_CurrentDayIncremental_NONCLUSTERED] ON [DefaultSchemaSetting].[CurrentDayIncremental];
					END;
					ELSE IF @IsCurrent = 0
					BEGIN
						IF EXISTS (SELECT * FROM SYS.INDEXES 
										WHERE NAME = N'IX_PreviousDayIncremental_References_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'PreviousDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							DROP INDEX [IX_PreviousDayIncremental_References_NONCLUSTERED] ON [DefaultSchemaSetting].[PreviousDayIncremental];
			
						IF EXISTS (SELECT * FROM SYS.INDEXES
										WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'PreviousDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							DROP INDEX IX_PreviousDayIncremental_NONCLUSTERED ON [DefaultSchemaSetting].[PreviousDayIncremental];
					END;
				END;
				GO

				ALTER   PROCEDURE [DefaultSchemaSetting].[CreateIncrementalIndex]
				@IsCurrent BIT
				AS
				BEGIN
					IF @IsCurrent = 1
					BEGIN
						IF NOT EXISTS (SELECT * FROM SYS.INDEXES 
										WHERE NAME = N'IX_CurrentDayIncremental_References_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'CurrentDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							CREATE NONCLUSTERED INDEX [IX_CurrentDayIncremental_References_NONCLUSTERED] 
							ON [DefaultSchemaSetting].[CurrentDayIncremental]
							(
								[PromoId] ASC,
								[ProductId] ASC
							)
							WITH 
							(
								PAD_INDEX = OFF, 
								STATISTICS_NORECOMPUTE = OFF, 
								SORT_IN_TEMPDB = OFF, 
								DROP_EXISTING = OFF, 
								ONLINE = OFF, 
								ALLOW_ROW_LOCKS = ON, 
								ALLOW_PAGE_LOCKS = ON
							) 
							ON [PRIMARY]
	
						IF NOT EXISTS (SELECT * FROM SYS.INDEXES 
										WHERE NAME = N'IX_CurrentDayIncremental_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'CurrentDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							CREATE NONCLUSTERED INDEX [IX_CurrentDayIncremental_NONCLUSTERED]
							ON [DefaultSchemaSetting].[CurrentDayIncremental] ([Id] ASC) 
							INCLUDE (
								[WEEK],
								[PromoId],
								[ProductId],
								[IncrementalQty],
								[LastChangeDate],
								[DemandCode],
								[DMDGroup]
							);
					END;
					ELSE IF @IsCurrent = 0
					BEGIN
						IF NOT EXISTS (SELECT * FROM SYS.INDEXES 
										WHERE NAME = N'IX_PreviousDayIncremental_References_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'PreviousDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							CREATE NONCLUSTERED INDEX [IX_PreviousDayIncremental_References_NONCLUSTERED] 
							ON [DefaultSchemaSetting].[PreviousDayIncremental]
							(
								[PromoId] ASC,
								[ProductId] ASC
							)
							WITH 
							(
								PAD_INDEX = OFF, 
								STATISTICS_NORECOMPUTE = OFF, 
								SORT_IN_TEMPDB = OFF, 
								DROP_EXISTING = OFF, 
								ONLINE = OFF, 
								ALLOW_ROW_LOCKS = ON, 
								ALLOW_PAGE_LOCKS = ON
							) 
							ON [PRIMARY]

						IF NOT EXISTS (SELECT * FROM SYS.INDEXES
										WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED'
											and object_id in (SELECT object_id FROM sys.objects WHERE NAME = 'PreviousDayIncremental' 
													and schema_id = (SELECT sys.schemas.schema_id FROM sys.schemas WHERE NAME = 'DefaultSchemaSetting')))
							CREATE NONCLUSTERED INDEX [IX_PreviousDayIncremental_NONCLUSTERED]
							ON [DefaultSchemaSetting].[PreviousDayIncremental] ([Id] ASC) 
							INCLUDE (
								[WEEK],
								[PromoId],
								[ProductId],
								[IncrementalQty],
								[LastChangeDate],
								[DemandCode],
								[DMDGroup]
							);
					END;
				END;
				GO
            ";
    }
}
