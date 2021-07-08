using Core.Data;
using Interfaces.Implementation.Import.FullImport;
using Looper.Parameters;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Host.TPM.Actions
{
    class FullXLSXImportPromoProductPluAction : FullXLSXImportPromoProductAction
    {
        public FullXLSXImportPromoProductPluAction(FullImportSettings settings, Guid promoId, Guid userId, Guid roleId)
            : base(settings, promoId, userId, roleId)
        {
        }

        protected override ImportResultFilesModel ApplyImport(IList<IEntity<Guid>> sourceRecords, out int successCount, out int warningCount, out int errorCount)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var products = context.Set<PromoProduct>().Include("Plu").Where(n => n.PromoId == promoId && !n.Disabled).ToList();
                foreach (ImportPromoProductPlu item in sourceRecords)
                {
                    var found = products.FirstOrDefault(x => x.Plu != null && x.Plu.PluCode == item.PluCode);
                    item.Plu = new PromoProduct2Plu() { PluCode = item.PluCode };
                    if (found != null)
                    {
                        item.EAN_PC = found.EAN_PC;
                    }
                }
            }
            return base.ApplyImport(sourceRecords, out successCount, out warningCount, out errorCount);
        }

        protected override bool IsFilterSuitable(IEntity<Guid> rec, out IList<string> errors)
        {
            errors = new List<string>();
            bool success = true;

            var promoProduct = rec as PromoProduct;
            if (string.IsNullOrWhiteSpace(promoProduct.Plu.PluCode))
            {
                errors.Add("Not filled PLU code");
                success = false;
            }
            if (promoProduct.EAN_PC == null)
            {
                errors.Add($"No match PLU code ({promoProduct.Plu.PluCode}) to Product");
                success = false;
            }
            promoProduct.ActualProductUOM = "PC";
            return success;
        }
    }
}
