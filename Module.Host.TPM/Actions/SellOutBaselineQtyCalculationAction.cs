using Core.Extensions;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    /// <summary>
    /// Класс для расчета значения SellOutBaselineQty после изменения записи в справочнике CoefficientSI2SO
    /// </summary>
    public class SellOutBaselineQtyCalculationAction : BaseAction
    {
        private readonly string OldBrandTechCode;
        private readonly string OldDemandCode;
        private readonly string BrandTechCode;
        private readonly string DemandCode;
        private readonly double CValue;
        private readonly string updateTemplate = "UPDATE BaseLine SET SellOutBaselineQTY = '{0}' WHERE Id = '{1}';";

        public SellOutBaselineQtyCalculationAction(string oldBrandTechCode, string oldDemandCode, string brandTechCode, string demandCode, double cValue)
        {
            OldBrandTechCode = oldBrandTechCode;
            OldDemandCode = oldDemandCode;
            BrandTechCode = brandTechCode;
            DemandCode = demandCode;
            CValue = cValue;
        }

        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    var ZREPs = context.Set<Product>().Where(x => x.BrandsegTech_code == BrandTechCode && !x.Disabled).Select(x => x.ZREP).ToList();
                    var baseLines = context.Set<BaseLine>().Where(x => x.DemandCode == DemandCode && ZREPs.Contains(x.Product.ZREP) && !x.Disabled).ToList();
                    
                    foreach (IEnumerable<BaseLine> items in baseLines.Partition(10000))
                    {
                        string updateScript = String.Join("", items.Select(y => String.Format(updateTemplate, y.SellInBaselineQTY * CValue, y.Id)));
                        context.Database.ExecuteSqlCommand(updateScript);
                    }

                    if (OldBrandTechCode != null && OldDemandCode != null)
                    {
                        ZREPs = context.Set<Product>().Where(x => x.BrandsegTech_code == OldBrandTechCode && !x.Disabled).Select(x => x.ZREP).ToList();
                        baseLines = context.Set<BaseLine>().Where(x => x.DemandCode == OldDemandCode && ZREPs.Contains(x.Product.ZREP) && !x.Disabled).ToList();

                        foreach (IEnumerable<BaseLine> items in baseLines.Partition(10000))
                        {
                            string updateScript = String.Join("", items.Select(y => String.Format(updateTemplate, 0, y.Id)));
                            context.Database.ExecuteSqlCommand(updateScript);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while calculation: {0}", e.ToString());
                Errors.Add(msg);
            }
        }
    }
}
