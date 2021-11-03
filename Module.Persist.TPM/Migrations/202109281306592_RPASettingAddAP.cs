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

            
            INSERT INTO {defaultSchema}.RPASettings(Json, Name) VALUES('{{'name': 'First test handler ', 'type':'Actuals_PLU','parametrs':[], 'roles': ['CMManager','Administrator','FunctionalExpert','KeyAccountManager','CustomerMarketing','SupportAdministrator'], " +
            "'templateColumns': [{'Order':0,'Field':'PromoID', 'Header':'PromoID', 'Quoting':false}, {'Order': 1,'Field': 'PLU', 'Header': 'PLU', 'Quoting': false},{'Order': 2,'Field': 'ActualProductPCQty', 'Header': 'ActualProductPCQty', 'Quoting': false}]}',"+
            "'Actual PLU Handler') "+

            "INSERT INTO {defaultSchema}.RPASettings(Json, Name) VALUES('{{'name': 'Second test handler', 'type':'Actuals_EAN_PC','parametrs':[], 'roles': ['CMManager','Administrator','FunctionalExpert','KeyAccountManager','CustomerMarketing','SupportAdministrator'], " +
            "'templateColumns': [{'Order':0,'Field':'PromoID', 'Header':'PromoID', 'Quoting':false}, {'Order': 1,'Field': 'EAN_PC', 'Header': 'PLU', 'Quoting': false},{'Order': 2,'Field': 'ActualProductPCQty', 'Header': 'ActualProductPCQty', 'Quoting': false}]}'," +
            "'Actual EAN PC Handler') " +
            
            "INSERT INTO {defaultSchema}.RPASettings(Json, Name) VALUES('{{'name': 'Third test handler', 'type':'Events','parametrs':[], 'roles': ['CMManager','Administrator','FunctionalExpert','KeyAccountManager','CustomerMarketing','SupportAdministrator'], " +
            "'templateColumns': [{'Order':0,'Field':'PromoID', 'Header':'PromoID', 'Quoting':false}, {'Order': 1,'Field': 'EventName', 'Header': 'EventName', 'Quoting': false}]}'," +
            "'Event Handler') ");
        }
        

        public override void Down()
        {
        }
    }
}
