namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_drop_OldPriceList : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchemaSetting", "Jupiter");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
           
        } 
        private string SqlString = @"
        IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'PriceList_Update_Trigger')
        BEGIN
            EXEC('DISABLE TRIGGER [DefaultSchemaSetting].[PriceList_Update_Trigger] ON [DefaultSchemaSetting].[PriceList]')
        END

        GO

        UPDATE [DefaultSchemaSetting].[PriceList]
        SET 
            [Disabled] = 1, 
            [DeletedDate] = GETDATE()
        WHERE 
            [EndDate] <= '2020-12-31' 
            AND [Disabled] <> 1";
    }
}
