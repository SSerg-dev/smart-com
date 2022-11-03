using Core.Data;
using Interfaces.Implementation.Import.FullImport;
using Module.Persist.TPM.Model.DTO;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportPromoProductPriceIncreasesUpliftAction : FullXLSXImportAction
    {
        private Guid promoId;
        private Guid userId;

        public FullXLSXUpdateImportPromoProductPriceIncreasesUpliftAction(FullImportSettings fullImportSettings, Guid promoId, Guid userId)
            : base(fullImportSettings)
        {
            this.promoId = promoId;
            this.userId = userId;
        }

        protected override int InsertDataToDatabase(IEnumerable<IEntity<Guid>> importedRecords, DatabaseContext databaseContext)
        {
            Promo promo = databaseContext.Set<Promo>().Find(promoId);
            // все PromoProductsCorrections для текущего промо
            // и все PromoProducts для текущего проммо
            var promoProductPIs = databaseContext.Set<PromoProductPriceIncrease>()
                .Include(g => g.ProductCorrectionPriceIncreases)
                .Where(x => x.PromoPriceIncreaseId == this.promoId)
                .ToList();
            // все импортируемые PromoProductsUplift
            var importedPromoProductViews = importedRecords.Cast<PromoProductPriceIncreasesView>();

            var importedPromoProductViewsGroups = importedPromoProductViews.GroupBy(x => x.ZREP);
            var importedPromoProductViewsRepeat = importedPromoProductViewsGroups.FirstOrDefault(x => x.Count() > 1);
            var importedPromoProductViewsRanges = importedPromoProductViews.Where(x => x.PlanProductUpliftPercent <= 0 || x.PlanProductUpliftPercent == null);
            var importedPromoProductViewsMisses = importedPromoProductViews.Where(x => promoProductPIs.FirstOrDefault(y => y.ZREP == x.ZREP) == null);

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
                    var currentPromoProductPI = promoProductPIs.FirstOrDefault(x => x.ZREP == importedPromoProductView.ZREP);
                    if (currentPromoProductPI.ProductCorrectionPriceIncreases != null)
                    {
                        if (currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().Disabled)
                        {
                            currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().Disabled = false;
                            currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().DeletedDate = null;
                        }
                        currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().PlanProductUpliftPercentCorrected = importedPromoProductView.PlanProductUpliftPercent;
                        currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().UserId = this.userId;
                        currentPromoProductPI.ProductCorrectionPriceIncreases.FirstOrDefault().UserName = databaseContext.Users.FirstOrDefault(x => x.Id == this.userId).Name;
                        //this.CreateChangesIncident(databaseContext, currentPromoProductCorrection);
                    }
                    else
                    {

                        var newPromoProductCorrection = new PromoProductCorrectionPriceIncrease
                        {
                            Disabled = false,
                            DeletedDate = null,
                            PlanProductUpliftPercentCorrected = importedPromoProductView.PlanProductUpliftPercent,
                            UserId = this.userId,
                            UserName = databaseContext.Users.FirstOrDefault(x => x.Id == userId).Name,
                            CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                            ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow)
                        };
                        currentPromoProductPI.ProductCorrectionPriceIncreases.Add(newPromoProductCorrection);
                        // this.CreateChangesIncident(databaseContext, newPromoProductCorrection);
                    }
                }
            }

            var saveChangesCount = databaseContext.SaveChanges();
            return saveChangesCount;
        }

        private void CreateChangesIncident(DatabaseContext databaseContext, PromoProductCorrectionPriceIncrease promoProductCorrection)
        {
            var changesIncidents = databaseContext.Set<ChangesIncident>();

            changesIncidents.Add(new ChangesIncident
            {
                Id = Guid.NewGuid(),
                DirectoryName = nameof(PromoProductCorrectionPriceIncrease),
                ItemId = promoProductCorrection.Id.ToString(),
                CreateDate = DateTimeOffset.Now,
                ProcessDate = null,
                DeletedDate = null,
                Disabled = false
            });
        }
    }
}
