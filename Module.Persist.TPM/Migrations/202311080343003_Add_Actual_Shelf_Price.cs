namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;

    public partial class Add_Actual_Shelf_Price : DbMigration
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
        private string SqlString = @"INSERT INTO [DefaultSchemaSetting].[RPASetting] 
                                    (Id, Json, Name) 
                                    VALUES(newid(), 
                                    '{ ""name"": ""Actual Shelf Price & Discount Handler"", 
        ""type"":""TLC_Actual_Shelf_Price_Discount"",""parametrs"":[], 
        ""roles"": [""CMManager"",""Administrator"",""FunctionalExpert"",""SupportAdministrator"",""CustomerMarketing"",""KeyAccountManager""], 
        ""templateColumns"": [{""Order"":0,""Field"":""PromoId"",""Header"":""PromoType"",""Quoting"":false}, 
        {""Order"": 1,""Field"": ""Mechanic"",""Header"": ""Mechanic"",""Quoting"": false}, 
        {""Order"": 2,""Field"": ""MechanicType"",""Header"": ""MechanicType"",""Quoting"": false},
        {""Order"": 3,""Field"": ""Discount"",""Header"": ""Discount"",""Quoting"": false},
        {""Order"": 4,""Field"": ""ShelfPrice"",""Header"": ""ShelfPrice"",""Quoting"": false}, 
        {""Order"": 5,""Field"": ""InvoiceNumber"",""Header"": ""InvoiceNumber"",""Quoting"": false},
        {""Order"": 6,""Field"": ""DocumentNumber"",""Header"": ""DocumentNumber"",""Quoting"": false}]}',
        'Actual Shelf Price & Discount Handler')";
    }
}
