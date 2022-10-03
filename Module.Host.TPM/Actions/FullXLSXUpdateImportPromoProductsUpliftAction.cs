using Core.Data;
using Interfaces.Implementation.Action;
using Interfaces.Implementation.Import.FullImport;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.Model.Interfaces;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportPromoProductsUpliftAction : FullXLSXImportAction
    {
        /// <summary>
        /// Id промо для которого загружается PromoProductUplift
        /// </summary>
        private Guid promoId;
        private Guid userId;
        private string TempId;
        private TPMmode TPMmode;

        public FullXLSXUpdateImportPromoProductsUpliftAction(FullImportSettings fullImportSettings, Guid promoId, Guid userId, string TempId, TPMmode tPMmode)
            : base(fullImportSettings)
        {
            this.promoId = promoId;
            this.userId = userId;
            this.TempId = TempId;
            this.TPMmode = tPMmode;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> importedRecords, DatabaseContext databaseContext)
        {
            Promo promo = databaseContext.Set<Promo>().Find(promoId);
            // все PromoProductsCorrections для текущего промо
            // и все PromoProducts для текущего проммо
            var promoProductCorrections = databaseContext.Set<PromoProductsCorrection>().Where(x => x.PromoProduct.PromoId == this.promoId /*&& x.TempId == this.TempId*/).ToList();
            var promoProducts = databaseContext.Set<PromoProduct>().Where(x => x.PromoId == this.promoId).ToList();
            // все импортируемые PromoProductsUplift
            var importedPromoProductViews = importedRecords.Cast<PromoProductsView>();

            var importedPromoProductViewsGroups = importedPromoProductViews.GroupBy(x => x.ZREP);
            var importedPromoProductViewsRepeat = importedPromoProductViewsGroups.FirstOrDefault(x => x.Count() > 1);
            var importedPromoProductViewsRanges = importedPromoProductViews.Where(x => x.PlanProductUpliftPercent <= 0 || x.PlanProductUpliftPercent == null);
            var importedPromoProductViewsMisses = importedPromoProductViews.Where(x => promoProducts.FirstOrDefault(y => y.ZREP == x.ZREP) == null);

            if (importedPromoProductViewsRepeat != null)
            {
                Errors.Add($"Records must not be repeated (Promo number: {promoId}, ZREP: {importedPromoProductViewsRepeat.Key})");
                HasErrors = true;
            }
            else if (importedPromoProductViewsRanges.Count() > 0)
            {
                foreach (var importedPromoProductViewsRange in importedPromoProductViewsRanges)
                {
                    Errors.Add($"Uplift must be greater than 0 (ZREP: {importedPromoProductViewsRange.ZREP})");
                    HasErrors = true;
                }
            }
            else if (importedPromoProductViewsMisses.Count() > 0)
            {
                var promoNumber = databaseContext.Set<Promo>().FirstOrDefault(x => x.Id == promoId).Number;
                foreach (var importedPromoProductViewsMiss in importedPromoProductViewsMisses)
                {
                    Errors.Add($"Promo (Number: {promo.Number}) does not contain Product (ZREP: {importedPromoProductViewsMiss.ZREP})");
                    HasErrors = true;
                }
            }
            else
            {
                foreach (var importedPromoProductView in importedPromoProductViews)
                {
                    var currentPromoProductCorrection = promoProductCorrections.FirstOrDefault(x => x.PromoProduct.ZREP == importedPromoProductView.ZREP);
                    if (currentPromoProductCorrection != null)
                    {
                        currentPromoProductCorrection.PlanProductUpliftPercentCorrected = importedPromoProductView.PlanProductUpliftPercent;
                        currentPromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        currentPromoProductCorrection.UserId = this.userId;
                        currentPromoProductCorrection.UserName = databaseContext.Users.FirstOrDefault(x => x.Id == this.userId).Name;
                        currentPromoProductCorrection.TempId = this.TempId;
                        this.CreateChangesIncident(databaseContext, currentPromoProductCorrection);
                    }
                    else
                    {
                        var currentPromoProduct = promoProducts.FirstOrDefault(x => x.ZREP == importedPromoProductView.ZREP);
                        if (currentPromoProduct != null)
                        {
                            var newPromoProductCorrection = new PromoProductsCorrection
                            {
                                Id = Guid.NewGuid(),
                                Disabled = false,
                                DeletedDate = null,
                                PromoProductId = currentPromoProduct.Id,
                                PlanProductUpliftPercentCorrected = importedPromoProductView.PlanProductUpliftPercent,
                                UserId = this.userId,
                                TempId = this.TempId,
                                UserName = databaseContext.Users.FirstOrDefault(x => x.Id == userId).Name,
                                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                                TPMmode = TPMmode
                            };
                            databaseContext.Set<PromoProductsCorrection>().Add(newPromoProductCorrection);
                        }
                        // this.CreateChangesIncident(databaseContext, newPromoProductCorrection);
                    }
                }
            }

            var saveChangesCount = databaseContext.SaveChanges();
            return saveChangesCount;
        }

        private void CreateChangesIncident(DatabaseContext databaseContext, PromoProductsCorrection promoProductCorrection)
        {
            var changesIncidents = databaseContext.Set<ChangesIncident>();

            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductsCorrection),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }
    }
}
