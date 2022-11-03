using Core.Data;
using Interfaces.Implementation.Import.FullImport;
using Module.Frontend.TPM.Controllers;
using Module.Persist.TPM.Model.Import;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Persist;
using Persist.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Actions
{
    class FullXLSXUpdateImportPromoProductCorrectionPriceIncreaseAction : FullXLSXImportAction
    {
        private readonly Guid _userId;
        private readonly TPMmode TPMmode;

        public FullXLSXUpdateImportPromoProductCorrectionPriceIncreaseAction(FullImportSettings fullImportSettings, Guid userId, TPMmode tPMmode)
            : base(fullImportSettings)
        {
            _userId = userId;
            TPMmode = tPMmode;
        }
        
        protected virtual bool IsFilterSuitable(ImportPromoProductsCorrection item, IEnumerable<ImportPromoProductsCorrection> importedPromoProductCorrections, out IList<string> errors, List<PromoProductPriceIncrease> promoProducts)
        {
            errors = new List<string>();
            var promoProduct = promoProducts.Where(x => x.ZREP == item.ProductZREP && x.PromoPriceIncrease.Promo.Number == item.PromoNumber).FirstOrDefault();

            var importedPromoProductCorrectionGroup = importedPromoProductCorrections.GroupBy(x => new { x.PromoNumber, x.ProductZREP }).FirstOrDefault(x => x.Key.PromoNumber == item.PromoNumber && x.Key.ProductZREP == item.ProductZREP);
            if (importedPromoProductCorrectionGroup.Count() > 1)
            {
                errors.Add($"Records must not be repeated (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
                return false;
            }
            else if (item.PlanProductUpliftPercentCorrected <= 0)
            {
                errors.Add($"Uplift must be greater than 0 and must not be empty (Promo number: {item.PromoNumber}, ZREP: {item.ProductZREP})");
                return false;
            }
            else if (promoProduct == null)
            {
                errors.Add($"No product with ZREP: {item.ProductZREP} found in Promo: {item.PromoNumber}");
                return false;
            }
            else
            {
                return true;
            }
        }
        protected int InsertDataToDatabase(IEnumerable<PromoProductCorrectionPriceIncrease> importedPromoProductCorrections, DatabaseContext databaseContext)
        {
            var currentUser = databaseContext.Set<User>().FirstOrDefault(x => x.Id == this._userId);
            var promoProductsCorrectionChangeIncidents = new List<PromoProductCorrectionPriceIncrease>();

            var promoProductIds = importedPromoProductCorrections.Select(ppc => ppc.PromoProductId);
            var promoProducts = databaseContext.Set<PromoProduct>().Where(pp => promoProductIds.Contains(pp.Id)).ToList();
            var promoIds = promoProducts.Select(pp => pp.PromoId);
            var promoes = databaseContext.Set<Promo>().Where(p => promoIds.Contains(p.Id)).ToList();

            List<PromoProductCorrectionPriceIncrease> promoProductsCorrections = new List<PromoProductCorrectionPriceIncrease>();

            foreach (var importedPromoProductCorrection in importedPromoProductCorrections)
            {
                var promoProduct = databaseContext.Set<PromoProduct>()
                        .FirstOrDefault(x => x.Id == importedPromoProductCorrection.PromoProductId && !x.Disabled);
                var promo = promoes.FirstOrDefault(q => promoProduct != null && q.Id == promoProduct.PromoId);

                if (promo.InOut.HasValue && promo.InOut.Value)
                {
                    Errors.Add($"Promo Product Correction was not imported for In-Out promo №{promo.Number}");
                    HasErrors = true;
                    continue;
                }

                var currentPromoProductCorrection = databaseContext.Set<PromoProductCorrectionPriceIncrease>()
                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                .FirstOrDefault(x => x.PromoProductId == importedPromoProductCorrection.PromoProductId && x.TempId == importedPromoProductCorrection.TempId && !x.Disabled && x.TPMmode == TPMmode);

                if (currentPromoProductCorrection != null)
                {
                    if (TPMmode == TPMmode.RS && currentPromoProductCorrection.PromoProduct.Promo.TPMmode == TPMmode.Current)
                    {
                        promoProductsCorrections = databaseContext.Set<PromoProductCorrectionPriceIncrease>()
                        .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                        .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                        .Where(x => x.PromoProduct.PromoId == currentPromoProductCorrection.PromoProduct.PromoId && !x.Disabled)
                        .ToList();
                        promoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, promoProductsCorrections);
                        currentPromoProductCorrection = promoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == currentPromoProductCorrection.PromoProduct.ZREP);
                    }

                    if (currentPromoProductCorrection.PromoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(currentPromoProductCorrection.TempId))
                    {
                        throw new ImportException("Promo Locked Update");
                    }

                    currentPromoProductCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                    currentPromoProductCorrection.ChangeDate = DateTimeOffset.Now;
                    currentPromoProductCorrection.UserId = this._userId;
                    currentPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                    if (TPMmode == TPMmode.Current)
                    {
                        var promoRS = databaseContext.Set<Promo>()
                        .Include(x => x.PromoProducts)
                        .FirstOrDefault(x => x.Number == promoProduct.Promo.Number && x.TPMmode == TPMmode.RS && !x.Disabled);
                        if (promoRS != null)
                        {
                            databaseContext.Set<Promo>().Remove(promoRS);
                            databaseContext.SaveChanges();

                            var currentPromoProductsCorrections = databaseContext.Set<PromoProductCorrectionPriceIncrease>()
                                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                                .Where(x => x.PromoProduct.PromoId == promoProduct.PromoId && !x.Disabled)
                                .ToList();
                            currentPromoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, currentPromoProductsCorrections);
                            var promoProductsCorrection = currentPromoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == promoProduct.ZREP);
                            promoProductsCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                            promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                            promoProductsCorrection.UserId = this._userId;
                            promoProductsCorrection.UserName = currentUser?.Name ?? string.Empty;
                        }
                    }
                    promoProductsCorrectionChangeIncidents.Add(currentPromoProductCorrection);
                }
                else
                {
                    if (TPMmode == TPMmode.Current)
                    {
                        promoProduct = databaseContext.Set<PromoProduct>()
                            .FirstOrDefault(x => x.Id == importedPromoProductCorrection.PromoProductId && !x.Disabled && x.TPMmode == TPMmode);

                        if (promoProduct.Promo.NeedRecountUplift == false && String.IsNullOrEmpty(importedPromoProductCorrection.TempId))
                        {
                            throw new ImportException("Promo Locked Update");
                        }
                        importedPromoProductCorrection.CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.UserId = this._userId;
                        importedPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                        databaseContext.Set<PromoProductCorrectionPriceIncrease>().Add(importedPromoProductCorrection);

                        var promoRS = databaseContext.Set<Promo>()
                        .Include(x => x.PromoProducts)
                        .FirstOrDefault(x => x.Number == promoProduct.Promo.Number && x.TPMmode == TPMmode.RS && !x.Disabled);

                        if (promoRS != null)
                        {
                            databaseContext.Set<Promo>().Remove(promoRS);
                            databaseContext.SaveChanges();

                            var currentPromoProductsCorrections = databaseContext.Set<PromoProductCorrectionPriceIncrease>()
                                .Include(g => g.PromoProduct.Promo.IncrementalPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoSupportPromoes)
                                .Include(g => g.PromoProduct.Promo.PromoProductTrees)
                                .Where(x => x.PromoProduct.PromoId == promoProduct.PromoId && !x.Disabled)
                                .ToList();
                            currentPromoProductsCorrections = RSmodeHelper.EditToPromoProductsCorrectionRS(databaseContext, currentPromoProductsCorrections);
                            var promoProductsCorrection = currentPromoProductsCorrections.FirstOrDefault(g => g.PromoProduct.ZREP == promoProduct.ZREP);
                            promoProductsCorrection.PlanProductUpliftPercentCorrected = importedPromoProductCorrection.PlanProductUpliftPercentCorrected;
                            promoProductsCorrection.ChangeDate = DateTimeOffset.Now;
                            promoProductsCorrection.UserId = this._userId;
                            promoProductsCorrection.UserName = currentUser?.Name ?? string.Empty;
                        }
                    }
                    if (TPMmode == TPMmode.RS)
                    {
                        var promoProductRS = databaseContext.Set<PromoProduct>()
                                        .FirstOrDefault(x => x.Promo.Number == promoProduct.Promo.Number && x.ZREP == promoProduct.ZREP && !x.Disabled && x.TPMmode == TPMmode.RS);
                        if (promoProductRS == null)
                        {
                            var currentPromo = databaseContext.Set<Promo>()
                                .Include(g => g.PromoSupportPromoes)
                                .Include(g => g.PromoProductTrees)
                                .Include(g => g.IncrementalPromoes)
                                .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                                .FirstOrDefault(p => p.Number == promo.Number && p.TPMmode == TPMmode.Current);
                            var promoRS = RSmodeHelper.EditToPromoRS(databaseContext, currentPromo);
                            promoProductRS = promoRS.PromoProducts
                                .FirstOrDefault(x => x.ZREP == promoProduct.ZREP);
                        }

                        importedPromoProductCorrection.TPMmode = TPMmode.RS;
                        importedPromoProductCorrection.PromoProduct = promoProductRS;
                        importedPromoProductCorrection.PromoProductId = promoProductRS.Id;
                        importedPromoProductCorrection.ChangeDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow);
                        importedPromoProductCorrection.UserId = this._userId;
                        importedPromoProductCorrection.UserName = currentUser?.Name ?? string.Empty;

                        databaseContext.Set<PromoProductCorrectionPriceIncrease>().Add(importedPromoProductCorrection);
                        promoProductsCorrections.Add(importedPromoProductCorrection);
                        promoProductsCorrectionChangeIncidents.Add(importedPromoProductCorrection);
                    };
                }
            }

            // Необходимо выполнить перед созданием инцидентов.
            databaseContext.SaveChanges();

            foreach (var promoProductsCorrection in promoProductsCorrectionChangeIncidents)
            {
                var currentPromoProductsCorrection = promoProductsCorrections.FirstOrDefault(x => x.PromoProductId == promoProductsCorrection.PromoProductId && !x.Disabled);
                if (currentPromoProductsCorrection != null)
                {
                    PromoProductCorrectionPriceIncreasesController.CreateChangesIncident(databaseContext.Set<ChangesIncident>(), currentPromoProductsCorrection);
                }
            }

            databaseContext.SaveChanges();
            return promoProductsCorrections.Count();
        }
    }
}
