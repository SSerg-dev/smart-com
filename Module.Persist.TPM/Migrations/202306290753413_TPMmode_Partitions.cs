namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TPMmode_Partitions : DbMigration
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

		private string SqlString = @"
				CREATE PARTITION FUNCTION partition_func_by_mode(INT) AS RANGE LEFT FOR VALUES(0);
				GO
				CREATE PARTITION SCHEME partition_schema_by_mode AS PARTITION partition_func_by_mode ALL TO ([PRIMARY])
				GO


				--PromoSupportPromo
				ALTER TABLE [DefaultSchemaSetting].[PromoSupportPromo] DROP CONSTRAINT [PK_DefaultSchemaSetting.PromoSupportPromo] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_PromoSupportPromo_Id_TPMmode ON [DefaultSchemaSetting].[PromoSupportPromo](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[PromoSupportPromo] ADD CONSTRAINT [PK_DefaultSchemaSetting.PromoSupportPromo] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];


				--PromoProductsCorrection
				ALTER TABLE [DefaultSchemaSetting].[PromoProductsCorrection] DROP CONSTRAINT [PK_DefaultSchemaSetting.PromoProductsCorrection] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_PromoProductsCorrection_Id_TPMmode ON [DefaultSchemaSetting].[PromoProductsCorrection](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[PromoProductsCorrection] ADD CONSTRAINT [PK_DefaultSchemaSetting.PromoProductsCorrection] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];


				--IncrementalPromo
				ALTER TABLE [DefaultSchemaSetting].[IncrementalPromo] DROP CONSTRAINT [PK_DefaultSchemaSetting.IncrementalPromo] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_IncrementalPromo_Id_TPMmode ON [DefaultSchemaSetting].[IncrementalPromo](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[IncrementalPromo] ADD CONSTRAINT [PK_DefaultSchemaSetting.IncrementalPromo] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];


				--PromoProductTree
				ALTER TABLE [DefaultSchemaSetting].[PromoProductTree] DROP CONSTRAINT [PK_DefaultSchemaSetting.PromoProductTree] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_PromoProductTree_Id_TPMmode ON [DefaultSchemaSetting].[PromoProductTree](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[PromoProductTree] ADD CONSTRAINT [PK_DefaultSchemaSetting.PromoProductTree] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];


				--PromoProduct
				ALTER TABLE [DefaultSchemaSetting].[PromoProductsCorrection] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoProductsCorrection_DefaultSchemaSetting.PromoProduct_PromoProductId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductPriceIncrease] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoProductPriceIncrease_DefaultSchemaSetting.PromoProduct_PromoProductId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] DROP CONSTRAINT [PK_DefaultSchemaSetting.PromoProduct] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_PromoProduct_Id_TPMmode ON [DefaultSchemaSetting].[PromoProduct](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] ADD CONSTRAINT [PK_DefaultSchemaSetting.PromoProduct] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];

				ALTER TABLE [DefaultSchemaSetting].[PromoProductsCorrection]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoProductsCorrection_DefaultSchemaSetting.PromoProduct_PromoProductId] FOREIGN KEY([PromoProductId])
				REFERENCES [DefaultSchemaSetting].[PromoProduct] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductsCorrection] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoProductsCorrection_DefaultSchemaSetting.PromoProduct_PromoProductId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductPriceIncrease]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoProductPriceIncrease_DefaultSchemaSetting.PromoProduct_PromoProductId] FOREIGN KEY([PromoProductId])
				REFERENCES [DefaultSchemaSetting].[PromoProduct] ([Id])
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductPriceIncrease] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoProductPriceIncrease_DefaultSchemaSetting.PromoProduct_PromoProductId]
				GO


				--Promo
				ALTER TABLE [DefaultSchemaSetting].[Sale] DROP CONSTRAINT [FK_DefaultSchemaSetting.Sale_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoUpliftFailIncident] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoUpliftFailIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoStatusChange] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoStatusChange_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductTree] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoProductTree_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoProduct_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] DROP CONSTRAINT [FK_DefaultSchemaSetting.Actual_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnRejectIncident] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoOnRejectIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnApprovalIncident] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoOnApprovalIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoCancelledIncident] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoCancelledIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoApprovedIncident] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoApprovedIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[Promo] DROP CONSTRAINT [FK_DefaultSchemaSetting.Promo_DefaultSchemaSetting.Promo_MasterPromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PreviousDayIncremental] DROP CONSTRAINT [FK_DefaultSchemaSetting.PreviousDayIncremental_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[IncrementalPromo] DROP CONSTRAINT [FK_DefaultSchemaSetting.IncrementalPromo_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoPriceIncrease] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoPriceIncrease_DefaultSchemaSetting.Promo_Id]
				GO

				ALTER TABLE [DefaultSchemaSetting].[CurrentDayIncremental] DROP CONSTRAINT [FK__CurrentDa__Promo__1DF06171]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoSupportPromo] DROP CONSTRAINT [FK_DefaultSchemaSetting.PromoSupportPromo_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[BTLPromo] DROP CONSTRAINT [FK_DefaultSchemaSetting.BTLPromo_DefaultSchemaSetting.Promo_PromoId]
				GO
				--
				ALTER TABLE [DefaultSchemaSetting].[Promo] DROP CONSTRAINT [PK_DefaultSchemaSetting.Promo] WITH ( ONLINE = OFF )
				GO

				CREATE UNIQUE CLUSTERED INDEX IX_Promo_Id_TPMmode ON [DefaultSchemaSetting].[Promo](TPMmode, Id) ON partition_schema_by_mode(TPMmode)

				ALTER TABLE [DefaultSchemaSetting].[Promo] ADD CONSTRAINT [PK_DefaultSchemaSetting.Promo] PRIMARY KEY NONCLUSTERED (Id) ON [PRIMARY];
				--

				ALTER TABLE [DefaultSchemaSetting].[Sale]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.Sale_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				GO

				ALTER TABLE [DefaultSchemaSetting].[Sale] CHECK CONSTRAINT [FK_DefaultSchemaSetting.Sale_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoUpliftFailIncident]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoUpliftFailIncident_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoUpliftFailIncident] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoUpliftFailIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoStatusChange]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoStatusChange_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoStatusChange] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoStatusChange_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductTree]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoProductTree_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProductTree] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoProductTree_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoProduct_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoProduct_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.Actual_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoProduct] CHECK CONSTRAINT [FK_DefaultSchemaSetting.Actual_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnRejectIncident]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoOnRejectIncident_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnRejectIncident] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoOnRejectIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnApprovalIncident]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoOnApprovalIncident_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoOnApprovalIncident] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoOnApprovalIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoCancelledIncident]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoCancelledIncident_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoCancelledIncident] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoCancelledIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoApprovedIncident]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoApprovedIncident_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoApprovedIncident] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoApprovedIncident_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[Promo]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.Promo_DefaultSchemaSetting.Promo_MasterPromoId] FOREIGN KEY([MasterPromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				GO

				ALTER TABLE [DefaultSchemaSetting].[Promo] CHECK CONSTRAINT [FK_DefaultSchemaSetting.Promo_DefaultSchemaSetting.Promo_MasterPromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PreviousDayIncremental]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PreviousDayIncremental_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PreviousDayIncremental] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PreviousDayIncremental_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[IncrementalPromo]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.IncrementalPromo_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[IncrementalPromo] CHECK CONSTRAINT [FK_DefaultSchemaSetting.IncrementalPromo_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoPriceIncrease]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoPriceIncrease_DefaultSchemaSetting.Promo_Id] FOREIGN KEY([Id])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoPriceIncrease] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoPriceIncrease_DefaultSchemaSetting.Promo_Id]
				GO

				ALTER TABLE [DefaultSchemaSetting].[CurrentDayIncremental]  WITH CHECK ADD FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoSupportPromo]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.PromoSupportPromo_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[PromoSupportPromo] CHECK CONSTRAINT [FK_DefaultSchemaSetting.PromoSupportPromo_DefaultSchemaSetting.Promo_PromoId]
				GO

				ALTER TABLE [DefaultSchemaSetting].[BTLPromo]  WITH CHECK ADD  CONSTRAINT [FK_DefaultSchemaSetting.BTLPromo_DefaultSchemaSetting.Promo_PromoId] FOREIGN KEY([PromoId])
				REFERENCES [DefaultSchemaSetting].[Promo] ([Id])
				ON DELETE CASCADE
				GO

				ALTER TABLE [DefaultSchemaSetting].[BTLPromo] CHECK CONSTRAINT [FK_DefaultSchemaSetting.BTLPromo_DefaultSchemaSetting.Promo_PromoId]
				GO";
	}
}
