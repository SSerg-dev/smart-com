using AutoMapper;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;
using Module.Persist.TPM.Enum;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.PromoStateControl.RoleStateMap;
using Module.Persist.TPM.Utils;
using Persist;
using Persist.Model.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Module.Frontend.TPM.FunctionalHelpers.RSPeriod
{
    public static class RSPeriodHelper
    {
        public static StartEndModel GetRSPeriod(DatabaseContext Context)
        {
            string weeks = Context.Set<Setting>().Where(g => g.Name == "RS_START_WEEKS").FirstOrDefault().Value;

            StartEndModel startEndModel = new StartEndModel
            {
                BudgetYear = TimeHelper.ThisBuggetYear()
            };

            if (Int32.TryParse(weeks, out int intweeks))
            {
                startEndModel.StartDate = TimeHelper.TodayStartDay().AddDays(intweeks * 7);
                startEndModel.EndDate = TimeHelper.ThisBuggetYearEnd();
                return startEndModel;
            }
            else
            {
                startEndModel.StartDate = DateTimeOffset.MinValue;
                return startEndModel;
            }
        }
        //public static void CreateRSPeriod(Promo promo, DatabaseContext Context)
        //{
        //    List<string> outStatuses = new List<string> { RSstateNames.WAITING, RSstateNames.APPROVED };
        //    RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
        //        .Include(g => g.Promoes)
        //        .FirstOrDefault(g => g.ClientTreeId == promo.ClientTreeKeyId && !g.Disabled && !outStatuses.Contains(g.RSstatus));

        //    StartEndModel startEndModel = GetRSPeriod(Context);
        //    RollingScenario rollingScenario = new RollingScenario();
        //    if (rollingScenarioExist == null)
        //    {
        //        ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == promo.ClientTreeId);
        //        rollingScenario = new RollingScenario
        //        {
        //            StartDate = startEndModel.StartDate,
        //            EndDate = startEndModel.EndDate,
        //            RSstatus = RSstateNames.DRAFT,
        //            ClientTree = client,
        //            Promoes = new List<Promo>()
        //        };
        //        rollingScenario.Promoes.Add(promo);
        //        Context.Set<RollingScenario>().Add(rollingScenario);
        //    }
        //    else
        //    {
        //        rollingScenarioExist.Promoes.Add(promo);
        //    }
        //    Context.SaveChanges();
        //}

        public static void MassApproveRSPeriod(List<RollingScenario> rollingScenarios, DatabaseContext Context)
        {

        }


        public static void CopyBackPromoes(List<Promo> promoesRS, DatabaseContext Context)
        {
            var cfgPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BrandTech, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Color, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RejectReason, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Event, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MasterPromo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoOnApprovalIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.CurrentDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenario, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            });
            var mapperPromoBack = cfgPromoBack.CreateMapper();
            //var cfgBTLPromoBack = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<BTLPromo, BTLPromo>()
            //        .ForMember(pTo => pTo.Id, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
            //        //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
            //        //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
            //        .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            //});
            //var mapperBTLPromoBack = cfgBTLPromoBack.CreateMapper();
            var cfgPromoSupportPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            });
            var mapperPromoSupportPromoBack = cfgPromoSupportPromoBack.CreateMapper();
            var cfgPromoProductTreeBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
            });
            var mapperPromoProductTreeBack = cfgPromoProductTreeBack.CreateMapper();
            var cfgIncrementalPromoBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
            });
            var mapperIncrementalPromoBack = cfgIncrementalPromoBack.CreateMapper();
            var cfgPromoProductBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
            });
            var mapperPromoProductBack = cfgPromoProductBack.CreateMapper();
            var cfgPromoProductsCorrectionBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
            });
            var mapperPromoProductsCorrectionBack = cfgPromoProductsCorrectionBack.CreateMapper();
            var cfgPromoProductWithCorrBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.MapFrom(f => f.PromoProductPriceIncreases.Where(g => !g.Disabled)))
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    .ForMember(pTo => pTo.TPMmode, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncreaseId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)));
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreaseId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            });
            var mapperPromoProductWithCorrBack = cfgPromoProductWithCorrBack.CreateMapper();
            var cfgPromoPriceIncreaseBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            });
            var mapperPromoPriceIncreaseBack = cfgPromoPriceIncreaseBack.CreateMapper();
            var cfgPromoProductPriceIncreaseBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncreaseId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.Ignore());
            });
            var mapperPromoProductPriceIncreaseBack = cfgPromoProductPriceIncreaseBack.CreateMapper();
            var cfgPromoProductCorrectionPriceIncreaseBack = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.Disabled, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreaseId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            });
            var mapperPromoProductCorrectionPriceIncreaseBack = cfgPromoProductCorrectionPriceIncreaseBack.CreateMapper();

            Guid promoStatusOnApproval = Context.Set<PromoStatus>().FirstOrDefault(g => g.SystemName == StateNames.ON_APPROVAL).Id;
            List<Guid> promoRSids = promoesRS.Select(h => h.Id).ToList();
            promoesRS = Context.Set<Promo>()
                    //.Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(g => g.PreviousDayIncrementals)
                    .Include(g => g.PromoStatusChanges)
                    .Include(g => g.PromoUpliftFailIncidents)
                    .Include(g => g.PromoOnApprovalIncidents)
                    .Include(g => g.PromoOnRejectIncidents)
                    .Include(g => g.PromoCancelledIncidents)
                    .Include(g => g.PromoApprovedIncidents)
                    .Include(g => g.CurrentDayIncrementals)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(f => f.ProductCorrectionPriceIncreases))
                .Where(x => promoRSids.Contains(x.Id)).ToList();

            List<int> promoRSnumbers = promoesRS.Select(h => h.Number).Cast<int>().ToList();
            List<Promo> promos = Context.Set<Promo>()
                    //.Include(g => g.BTLPromoes)
                    .Include(g => g.PromoSupportPromoes)
                    .Include(g => g.PromoProductTrees)
                    .Include(g => g.IncrementalPromoes)
                    .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                    .Include(g => g.PromoPriceIncrease.PromoProductPriceIncreases.Select(f => f.ProductCorrectionPriceIncreases))
                    .Where(x => promoRSnumbers.Contains((int)x.Number) && x.TPMmode == TPMmode.Current).ToList();
            List<int> numbers = promos.Select(g => g.Number).Cast<int>().ToList();
            foreach (Promo promoRS in promoesRS.ToList())
            {
                if (numbers.Contains((int)promoRS.Number)) // существующий
                {
                    Promo promo = promos.FirstOrDefault(g => g.Number == promoRS.Number);
                    mapperPromoBack.Map(promoRS, promo);
                    //foreach (BTLPromo bTLPromoRS in promoRS.BTLPromoes)
                    //{
                    //    if (promo.BTLPromoes.Select(g => g.BTLId).Contains(bTLPromoRS.BTLId)) // существующий btlpromo
                    //    {
                    //        BTLPromo bTLPromo = promo.BTLPromoes.FirstOrDefault(g => g.BTLId == bTLPromoRS.BTLId  && !g.Disabled);
                    //        mapperBTLPromoBack.Map(bTLPromoRS, bTLPromo);

                    //    }
                    //    else // новый btlpromo
                    //    {
                    //        bTLPromoRS.PromoId = promo.Id;
                    //        bTLPromoRS.Promo = promo;
                    //        bTLPromoRS.TPMmode = TPMmode.Current;
                    //    }
                    //}
                    foreach (PromoSupportPromo promoSupportPromoRS in promoRS.PromoSupportPromoes)
                    {
                        if (promo.PromoSupportPromoes.Select(g => g.PromoSupportId).Contains(promoSupportPromoRS.PromoSupportId)) // существующий PromoSupportPromo
                        {
                            PromoSupportPromo promoSupportPromo = promo.PromoSupportPromoes.FirstOrDefault(g => g.PromoSupportId == promoSupportPromoRS.PromoSupportId && !g.Disabled);
                            mapperPromoSupportPromoBack.Map(promoSupportPromoRS, promoSupportPromo);

                        }
                        else // новый PromoSupportPromo
                        {
                            promoSupportPromoRS.PromoId = promo.Id;
                            promoSupportPromoRS.Promo = promo;
                            promoSupportPromoRS.TPMmode = TPMmode.Current;
                        }
                    }
                    foreach (IncrementalPromo incrementalPromoRS in promoRS.IncrementalPromoes)
                    {
                        if (promo.IncrementalPromoes.Select(g => g.ProductId).Contains(incrementalPromoRS.ProductId)) // существующий IncrementalPromo
                        {
                            IncrementalPromo incrementalPromo = promo.IncrementalPromoes.FirstOrDefault(g => g.ProductId == incrementalPromoRS.ProductId && !g.Disabled);
                            mapperIncrementalPromoBack.Map(incrementalPromoRS, incrementalPromo);
                            MoveFromRSChangesIncident(Context.Set<ChangesIncident>(), nameof(IncrementalPromo), incrementalPromo.Id, incrementalPromoRS.Id);
                        }
                        else // новый IncrementalPromo
                        {
                            incrementalPromoRS.PromoId = promo.Id;
                            incrementalPromoRS.Promo = promo;
                            incrementalPromoRS.TPMmode = TPMmode.Current;
                        }
                    }
                    foreach (PromoProductTree promoProductTreeRS in promoRS.PromoProductTrees)
                    {
                        if (promo.PromoProductTrees.Select(g => g.ProductTreeObjectId).Contains(promoProductTreeRS.ProductTreeObjectId)) // существующий PromoProductTree
                        {
                            PromoProductTree promoProductTree = promo.PromoProductTrees.FirstOrDefault(g => g.ProductTreeObjectId == promoProductTreeRS.ProductTreeObjectId && !g.Disabled);
                            mapperPromoProductTreeBack.Map(promoProductTreeRS, promoProductTree);

                        }
                        else // новый PromoProductTree
                        {
                            promoProductTreeRS.PromoId = promo.Id;
                            promoProductTreeRS.Promo = promo;
                            promoProductTreeRS.TPMmode = TPMmode.Current;
                        }
                    }
                    var Zreps = promo.PromoProducts.Where(g => !g.Disabled).Select(f => f.ZREP);
                    var ZrepsRS = promoRS.PromoProducts.Where(g => !g.Disabled).Select(f => f.ZREP);
                    var OutZreps = new List<string>();
                    foreach (var zrep in Zreps)
                    {
                        if (!ZrepsRS.Contains(zrep))
                        {
                            OutZreps.Add(zrep);
                        }
                    }
                    foreach (PromoProduct promoProductRS in promoRS.PromoProducts.Where(g => !g.Disabled).ToList())
                    {
                        if (promo.PromoProducts.Select(g => g.ProductId).Contains(promoProductRS.ProductId)) // существующий PromoProduct
                        {
                            PromoProduct promoProduct = promo.PromoProducts.FirstOrDefault(g => g.ProductId == promoProductRS.ProductId && !g.Disabled);

                            if (promoProduct != null && !OutZreps.Contains(promoProductRS.ZREP))
                            {
                                foreach (PromoProductsCorrection promoProductsCorrectionRS in promoProductRS.PromoProductsCorrections.ToList())
                                {
                                    PromoProductsCorrection promoProductsCorrection = promoProduct.PromoProductsCorrections.FirstOrDefault(g => !g.Disabled);
                                    if (promoProductsCorrection != null)
                                    {
                                        mapperPromoProductsCorrectionBack.Map(promoProductsCorrectionRS, promoProductsCorrection);
                                        MoveFromRSChangesIncident(Context.Set<ChangesIncident>(), nameof(PromoProductsCorrection), promoProductsCorrection.Id, promoProductsCorrectionRS.Id);
                                        //promoProductRS.PromoProductsCorrections.Remove(promoProductsCorrectionRS);
                                        Context.Set<PromoProductsCorrection>().Remove(promoProductsCorrectionRS);
                                    }
                                    else
                                    {
                                        promoProductsCorrectionRS.TPMmode = TPMmode.Current;
                                        promoProductsCorrectionRS.PromoProductId = promoProduct.Id;
                                        promoProductsCorrectionRS.PromoProduct = promoProduct;
                                    }

                                }
                                mapperPromoProductBack.Map(promoProductRS, promoProduct);
                                //priceIncrease
                                PromoProductPriceIncrease promoProductPI = promo.PromoPriceIncrease.PromoProductPriceIncreases
                                    .FirstOrDefault(f => f.PromoProductId == promoProduct.Id && !f.Disabled);
                                PromoProductPriceIncrease promoProductPIRS = promoRS.PromoPriceIncrease.PromoProductPriceIncreases
                                    .FirstOrDefault(f => f.ZREP == promoProductPI.ZREP && !f.Disabled);

                                PromoProductCorrectionPriceIncrease promoProductCorrectionPIRS = promoRS.PromoPriceIncrease.PromoProductPriceIncreases
                                    .FirstOrDefault(f => f.ZREP == promoProductPI.ZREP && !f.Disabled).ProductCorrectionPriceIncreases
                                    .FirstOrDefault(f => !f.Disabled);

                                PromoProductCorrectionPriceIncrease promoProductsCorrectionPI = promoRS.PromoPriceIncrease.PromoProductPriceIncreases
                                    .FirstOrDefault(f => f.ZREP == promoProduct.ZREP && !f.Disabled).ProductCorrectionPriceIncreases
                                    .FirstOrDefault(f => !f.Disabled);

                                if (promoProductsCorrectionPI != null)
                                {
                                    mapperPromoProductCorrectionPriceIncreaseBack.Map(promoProductCorrectionPIRS, promoProductsCorrectionPI);
                                    //MoveFromRSChangesIncident(Context.Set<ChangesIncident>(), nameof(PromoProductsCorrection), promoProductsCorrection.Id, promoProductCorrectionPIRS.Id);
                                    Context.Set<PromoProductCorrectionPriceIncrease>().Remove(promoProductCorrectionPIRS);
                                }
                                else
                                {
                                    if (promoProductCorrectionPIRS != null)
                                    {
                                        promoProductCorrectionPIRS.PromoProductPriceIncreaseId = promoProductPI.Id;
                                        promoProductCorrectionPIRS.PromoProductPriceIncrease = promoProductPI;
                                    }
                                }


                                mapperPromoProductPriceIncreaseBack.Map(promoProductPIRS, promoProductPI);

                                //promoRS.PromoProducts.Remove(promoProductRS);
                                Context.Set<PromoProductPriceIncrease>().Remove(promoProductPIRS);
                            }
                            else if (promoProduct == null && !OutZreps.Contains(promoProductRS.ZREP))
                            {
                                PromoProduct promoProduct1 = mapperPromoProductWithCorrBack.Map<PromoProduct>(promoProductRS);
                                promoProduct1.Promo = promo;
                                promoProduct1.PromoId = promo.Id;
                                foreach (PromoProductPriceIncrease productPriceIncrease in promoProduct1.PromoProductPriceIncreases)
                                {
                                    productPriceIncrease.PromoPriceIncrease = promo.PromoPriceIncrease;
                                    productPriceIncrease.PromoPriceIncreaseId = promo.PromoPriceIncrease.Id;
                                }
                                Context.Set<PromoProduct>().Add(promoProduct1);
                                Context.SaveChanges();
                            }
                        }
                        else // новый PromoProduct
                        {
                            promoProductRS.PromoId = promo.Id;
                            promoProductRS.Promo = promo;
                            promoProductRS.TPMmode = TPMmode.Current;
                        }
                    }
                    if (OutZreps.Count > 0)
                    {
                        foreach (var zrep in OutZreps)
                        {
                            PromoProduct promoProduct = promo.PromoProducts.FirstOrDefault(f => f.ZREP == zrep && !f.Disabled);
                            promoProduct.DeletedDate = DateTimeOffset.Now;
                            promoProduct.Disabled = true;
                            foreach (var correction in promoProduct.PromoProductsCorrections)
                            {
                                correction.DeletedDate = DateTimeOffset.Now;
                                correction.Disabled = true;
                            }
                            if (promoProduct.PromoProductPriceIncreases != null)
                            {
                                PromoProductPriceIncrease promoProductPriceIncrease = promoProduct.PromoProductPriceIncreases.FirstOrDefault(f => !f.Disabled);
                                promoProductPriceIncrease.DeletedDate = DateTimeOffset.Now;
                                promoProductPriceIncrease.Disabled = true;
                                foreach (var correction in promoProductPriceIncrease.ProductCorrectionPriceIncreases)
                                {
                                    correction.DeletedDate = DateTimeOffset.Now;
                                    correction.Disabled = true;
                                }
                            }
                        }
                    }
                    promo.PromoStatusId = promoStatusOnApproval;
                    //promoesRS.Remove(promoRS); - нельзя сделать
                    Context.Set<Promo>().Remove(promoRS); // не отследит EF
                                                          //ChangeStatusOnApproval(Context, promo);
                }
                else // новый
                {
                    promoRS.TPMmode = TPMmode.Current;
                    promoRS.RollingScenario = null; // unlink
                    foreach (var item in promoRS.IncrementalPromoes)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoSupportPromoes)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoProductTrees)
                    {
                        item.TPMmode = TPMmode.Current;
                    }
                    foreach (var item in promoRS.PromoProducts)
                    {
                        item.TPMmode = TPMmode.Current;
                        foreach (var item2 in item.PromoProductsCorrections)
                        {
                            item2.TPMmode = TPMmode.Current;
                        }
                    }
                    promoRS.PromoStatusId = promoStatusOnApproval;
                    //ChangeStatusOnApproval(Context, promoRS);
                }



                // Context.SaveChanges(); //удалить
            }

            Context.SaveChanges();
        }
        public static void MoveFromRSChangesIncident(DbSet<ChangesIncident> changesIncidents, string directoryName, Guid id, Guid oldId)
        {
            List<ChangesIncident> changesIncidents1 = changesIncidents.Where(g => g.ItemId == oldId.ToString() && g.DirectoryName == directoryName).ToList();
            foreach (var item in changesIncidents1)
            {
                item.ItemId = id.ToString();
            }
        }
        public static void RemoveOldCreateNewRSPeriodML(int clientId, Guid bufferId, DatabaseContext Context)
        {
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            RollingScenario rollingScenarioExist = Context.Set<RollingScenario>()
                .Include(g => g.Promoes)
                .FirstOrDefault(g => g.ClientTreeId == client.Id && !g.Disabled);


            if (rollingScenarioExist == null)
            {
                CreateMLRSperiod(clientId, bufferId, Context);
            }
            else
            {
                ScenarioHelper.DeleteScenarioPeriod(rollingScenarioExist.Id, Context);
                CreateMLRSperiod(clientId, bufferId, Context);
            }
            Context.SaveChanges();
        }
        private static void CreateMLRSperiod(int clientId, Guid bufferId, DatabaseContext Context)
        {
            List<PromoStatus> promoStatuses = Context.Set<PromoStatus>().Where(g => !g.Disabled).ToList();
            StartEndModel startEndModel = GetRSPeriod(Context);
            ClientTree client = Context.Set<ClientTree>().FirstOrDefault(g => g.ObjectId == clientId && g.EndDate == null);
            RollingScenario rollingScenario = new RollingScenario
            {
                StartDate = startEndModel.StartDate,
                EndDate = startEndModel.EndDate,
                RSstatus = RSstateNames.WAITING,
                ClientTree = client,
                IsMLmodel = true,
                FileBufferId = bufferId
            };
            Context.Set<RollingScenario>().Add(rollingScenario);
        }
    }
}
