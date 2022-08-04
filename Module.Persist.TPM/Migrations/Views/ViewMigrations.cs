namespace Module.Persist.TPM.Migrations.Views
{
    /// <summary>
    /// Имя схемы менять на DefaultSchemaSetting
    /// </summary>
    public static class ViewMigrations
    {
        public static string GetPromoGridViewString(string defaultSchema)
        {            
            return SqlString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string SqlString = @" 
             
            ";

        public static string GetPromoInsertTriggerString(string defaultSchema)
        {
            return PromoInsertTriggerSqlString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string PromoInsertTriggerSqlString = @" 
             ALTER TRIGGER [DefaultSchemaSetting].[Promo_increment_number] ON [DefaultSchemaSetting].[Promo] AFTER INSERT AS
                BEGIN

					If (SELECT Number FROM INSERTED) > 0 
					Begin
						Return
					End
    				UPDATE Promo SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM Promo WHERE Number < 999999), 0) + 1) FROM Inserted WHERE Promo.Id = Inserted.Id;
                END
            ";
    }
}
