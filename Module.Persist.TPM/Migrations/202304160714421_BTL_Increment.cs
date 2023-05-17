namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;
    
    public partial class BTL_Increment : DbMigration
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
            ALTER TRIGGER [DefaultSchemaSetting].[BTL_Increment_Number] ON [DefaultSchemaSetting].[BTL] AFTER INSERT AS

            BEGIN
	            UPDATE b
		            SET 
			            b.Number = (SELECT ISNULL((SELECT MAX(Number) FROM [DefaultSchemaSetting].[BTL] WHERE Number < 999999), 0) + 1)
		            FROM
			            [DefaultSchemaSetting].[BTL] b
			            INNER JOIN Inserted i ON i.Id = b.Id
            END;
        ";
    }
}
