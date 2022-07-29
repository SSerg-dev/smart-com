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
    }
}
