namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPASettingAddAP : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql($@"

            DECLARE @ItemId UNIQUEIDENTIFIER

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPASettings', 'GetRPASettings')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPASettings' AND Action = 'GetRPASettings'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPASettings', 'GetRPAs')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPASettings' AND Action = 'GetRPAs'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'GetRPA')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'GetRPA'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'   

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'GetFilteredData')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'GetFilteredData'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'
            
            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'Post')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'Post'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'UploadFile')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'UploadFile'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'DownloadFile')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'DownloadFile'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'SaveRPA')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'SaveRPA'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'

            INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'RPAs', 'DownloadTemplateXLSX')
            SELECT @ItemId = Id FROM[{defaultSchema}].[AccessPoint] where Resource = 'RPAs' AND Action = 'DownloadTemplateXLSX'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from {defaultSchema}.Role WHERE SystemName = 'Administrator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'SupportAdministator'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'KeyAccountManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandPlanning'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CMManager'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'FunctionalExpert'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'DemandFinance'
            INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM {defaultSchema}.Role WHERE SystemName = 'CustomerMarketing'
            " + 
            "\n INSERT [Jupiter].[RPASetting] ([Id], [Json], [Name]) VALUES (N'a069e9ce-a653-4552-8d31-439e8ab3431f', N'{ \"name\": \"Обработчик первый тестовый\", \"type\":\"Actuals_PLU\",\"parametrs\":[], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"KeyAccountManager\",\"CustomerMarketing\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoID\",\"Header\":\"PromoID\",\"Quoting\":false" +
            "}, {\"Order\": 1,\"Field\": \"PLU\",\"Header\": \"PLU\",\"Quoting\": false" +
            "},{\"Order\": 2,\"Field\": \"ActualProductPCQty\",\"Header\": \"ActualProductPCQty\",\"Quoting\": false" +
            "}]}', N'Actual PLU Handler')" +
            "\nGO\n" +
            "INSERT[Jupiter].[RPASetting]" +
            "    ([Id], [Json], [Name]) VALUES(N'6ce067d4-d382-44aa-a1c3-b7884521f639', N'{\"name\": \"Обработчик первый тестовый\", \"type\":\"Actuals_EAN_PC\",\"parametrs\":[], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"KeyAccountManager\",\"CustomerMarketing\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoID\",\"Header\":\"PromoID\",\"Quoting\":false" +
            "}, {" +
            "    \"Order\": 1,\"Field\": \"EAN_PC\",\"Header\": \"EAN_PC\",\"Quoting\": false" +
            "},{" +
            "    \"Order\": 2,\"Field\": \"ActualProductPCQty\",\"Header\": \"ActualProductPCQty\",\"Quoting\": false" +
            "}]}', N'Actual EAN PC Handler')" +
            "\nGO\n" +
            "INSERT [Jupiter].[RPASetting] ([Id], [Json], [Name]) VALUES(N'67b60777-0bee-40f8-a32b-c681c0fbb237', N'{\"name\": \"Обработчик первый тестовый\", \"type\":\"Events\", \"parametrs\": [], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"KeyAccountManager\",\"CustomerMarketing\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoNumber\",\"Header\":\"PromoNumber\",\"Quoting\":false" +
            "}, {" +
            "    \"Order\": 1,\"Field\": \"EventName\",\"Header\": \"EventName\",\"Quoting\": false" +
            "}]}', N'Event Handler')" +
            "\nGO\n" +
            "INSERT [Jupiter].[RPASetting] ([Id], [Json], [Name]) VALUES(N'b1a5de0d-bbe6-4076-8274-e04c007d7c3a', N'{\"name\": \"Fifth test handler\", \"type\":\"PromoSupport\", \"parametrs\": [], \"roles\": [\"CMManager\",\"Administrator\",\"FunctionalExpert\",\"KeyAccountManager\",\"CustomerMarketing\",\"SupportAdministrator\"], \"templateColumns\": [{\"Order\":0,\"Field\":\"PromoSupportId\",\"Header\":\"PromoSupportId\",\"Quoting\":false" +
            "}, {" +
            "    \"Order\": 1,\"Field\": \"ExternalCode\",\"Header\": \"ExternalCode\",\"Quoting\": false" +
            "}, {" +
            "    \"Order\": 2,\"Field\": \"Quantity\",\"Header\": \"Quantity\",\"Quoting\": false" +
            "}]}', N'Promo Support Handler')" +
            "\nGO\n");
        }
        

        public override void Down()
        {
        }
    }
}
