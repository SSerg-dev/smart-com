namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_Insert_Triggers : DbMigration
    {
        public override void Up()
        {

            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($@"
                ALTER TRIGGER [{defaultSchema}].[Promo_increment_number] ON [{defaultSchema}].[Promo] AFTER INSERT AS
                BEGIN
                    UPDATE Promo SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM Promo WHERE Number < 999999), 0) + 1) FROM Inserted WHERE Promo.Id = Inserted.Id;
                END
        
                GO

                ALTER TRIGGER [{defaultSchema}].[PromoSupport_Increment_Number] ON [{defaultSchema}].[PromoSupport] AFTER INSERT AS
                BEGIN 
	                UPDATE PromoSupport SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM PromoSupport WHERE Number < 999999), 0) + 1) FROM Inserted WHERE PromoSupport.Id = Inserted.Id; 
                END

                GO

                ALTER   TRIGGER [{defaultSchema}].[NonPromoSupport_Increment_Number] ON [{defaultSchema}].[NonPromoSupport] AFTER INSERT AS
                BEGIN 
	                UPDATE NonPromoSupport SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM NonPromoSupport WHERE Number < 999999), 0) + 1) FROM Inserted WHERE NonPromoSupport.Id = Inserted.Id;
                END
                ");
        }
        
        public override void Down()
        {
        }
    }
}
