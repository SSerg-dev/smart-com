namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RollingScenario : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.RollingScenario",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        RSId = c.Int(),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ExpirationDate = c.DateTimeOffset(precision: 7),
                        CreatorId = c.Guid(nullable: false),
                        CreatorLogin = c.String(),
                        PromoStatusId = c.Guid(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.RSId, unique: true)
                .Index(t => t.PromoStatusId)
                .Index(t => t.ClientTreeId);
            
            AddColumn($"{defaultSchema}.Promo", "RollingScenarioId", c => c.Guid());
            CreateIndex($"{defaultSchema}.Promo", "RollingScenarioId");
            AddForeignKey($"{defaultSchema}.Promo", "RollingScenarioId", $"{defaultSchema}.RollingScenario", "Id");
            AddForeignKey($"{defaultSchema}.RollingScenario", "ClientTreeId", $"{defaultSchema}.ClientTree", "Id");
            AddForeignKey($"{defaultSchema}.RollingScenario", "PromoStatusId", $"{defaultSchema}.PromoStatus", "Id");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.Promo", "RollingScenarioId", $"{defaultSchema}.RollingScenario");
            DropForeignKey($"{defaultSchema}.RollingScenario", "PromoStatusId", $"{defaultSchema}.PromoStatus");
            DropForeignKey($"{defaultSchema}.RollingScenario", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropIndex($"{defaultSchema}.RollingScenario", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.RollingScenario", new[] { "PromoStatusId" });
            DropIndex($"{defaultSchema}.RollingScenario", new[] { "RSId" });
            DropIndex($"{defaultSchema}.Promo", new[] { "RollingScenarioId" });
            DropColumn($"{defaultSchema}.Promo", "RollingScenarioId");
            DropTable($"{defaultSchema}.RollingScenario");
        }
        private string SqlString = @" 
            CREATE SEQUENCE [DefaultSchemaSetting].[RollingScenarioSequence] 
                 AS [int]
                 START WITH 0
                 INCREMENT BY 1
                 MAXVALUE 2147483647

                GO

            CREATE OR ALTER   TRIGGER [DefaultSchemaSetting].[RollingScenario_increment_RSId] ON [DefaultSchemaSetting].[RollingScenario] AFTER INSERT AS

                    BEGIN
	                    UPDATE cp
		                    SET 
			                    cp.RSId = NEXT VALUE FOR DefaultSchemaSetting.RollingScenarioSequence
		                    FROM
			                    DefaultSchemaSetting.RollingScenario cp
			                    INNER JOIN Inserted i ON i.Id = cp.Id
                    END;
            ";
    }
}
