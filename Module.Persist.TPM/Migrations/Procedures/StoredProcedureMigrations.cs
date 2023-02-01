using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Migrations.Procedures
{
    public static class StoredProcedureMigrations
    {
        public static string UpdateSICalculation(string defaultSchema)
        {
            return SqlString.Replace("DefaultSchemaSetting", defaultSchema);
        }
        private static readonly string SqlString = @"
        ALTER   PROCEDURE [DefaultSchemaSetting].[SI_Calculation] AS
           BEGIN
               UPDATE BaseLine SET 
			        SellInBaselineQTY = t.SellInQty
		        FROM
			        (SELECT b.Id AS baseLineId, b.SellInBaseLineQTY AS SellInQty
			        FROM [DefaultSchemaSetting].[BaseLine] AS b
			        WHERE b.NeedProcessing = 1 AND b.Disabled = 0 AND YEAR(b.StartDate) <> 9999) AS t
		        WHERE Id = baseLineId
           END
        GO
        ";
    }
}
