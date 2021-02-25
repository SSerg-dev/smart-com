namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ANAPLAN_BASELINE_CLIENTS_Setting : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            if (defaultSchema.Equals("Jupiter"))
            {
                Sql($@"
                    {AddSetting}
                    go
                ");
            }
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            if (defaultSchema.Equals("Jupiter"))
            {
                Sql($@"
                    {DeleteSetting}
                    go
                ");
            }
        }

        private string AddSetting =
        @"
            if not exists(select * from [Jupiter].[Setting] where [Name] = 'ANAPLAN_BASELINE_CLIENTS')
                insert into [Jupiter].[Setting]
                           ([Id]
                           ,[Name]
                           ,[Type]
                           ,[Value]
                           ,[Description])
                     values
                           (newid()
                           ,'ANAPLAN_BASELINE_CLIENTS'
                           ,'String'
                           ,''
                           ,'Anaplan DMDGroup codes')
        ";

        private string DeleteSetting =
        @"
            if exists(select * from [Jupiter].[Setting] where [Name] = 'ANAPLAN_BASELINE_CLIENTS')
                delete from [Jupiter].[Setting] where [Name] = 'ANAPLAN_BASELINE_CLIENTS'
        ";
    }
}
