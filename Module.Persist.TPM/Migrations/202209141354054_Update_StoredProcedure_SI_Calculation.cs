namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using Module.Persist.TPM.Migrations.Procedures;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_StoredProcedure_SI_Calculation : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            Sql(StoredProcedureMigrations.UpdateSICalculation(defaultSchema));

        }
        
        public override void Down()
        {
            
        }
    }
}
