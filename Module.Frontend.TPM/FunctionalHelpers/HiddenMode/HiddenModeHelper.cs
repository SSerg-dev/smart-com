using AutoMapper;
using Core.History;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.MongoDB;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Frontend.TPM.FunctionalHelpers.HiddenMode
{
    public static class HiddenModeHelper
    {
        public static List<Promo> HidePromoes(DatabaseContext Context, List<Promo> promoes)
        {
            foreach (Promo promo in promoes)
            {
                promo.TPMmode = TPMmode.Hidden;
                foreach (PromoProductTree promoProductTree in promo.PromoProductTrees)
                {
                    promoProductTree.TPMmode = TPMmode.Hidden;
                }
                foreach (BTLPromo bTLPromo in promo.BTLPromoes)
                {
                    bTLPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (IncrementalPromo incrementalPromo in promo.IncrementalPromoes)
                {
                    incrementalPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (PromoSupportPromo supportPromo in promo.PromoSupportPromoes)
                {
                    supportPromo.TPMmode = TPMmode.Hidden;
                }
                foreach (PromoProduct promoProduct in promo.PromoProducts)
                {
                    promoProduct.TPMmode = TPMmode.Hidden;
                    foreach (PromoProductsCorrection promoProductsCorrection in promoProduct.PromoProductsCorrections)
                    {
                        promoProductsCorrection.TPMmode = TPMmode.Hidden;
                    }
                }
            }

            Context.SaveChanges();
            return promoes;
        }
        public static List<Promo> SetTypePromoes(List<Promo> promoes, TPMmode tPMmode)
        {
            foreach (Promo promo in promoes)
            {
                promo.TPMmode = tPMmode;
            }

            return promoes;
        }
        public static List<Promo> CopyPromoesToHidden(DatabaseContext Context, List<Promo> promoes, SavedScenario savedScenario, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.MapFrom(f => f.BTLPromoes.Where(g => !g.Disabled)))//filter
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
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.MapFrom(f => f.PromoSupportPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.MapFrom(f => f.PromoProductTrees.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => { opt.Condition(c => c.InOut == true); opt.MapFrom(f => f.IncrementalPromoes.Where(g => !g.Disabled)); })
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.MapFrom(f => f.PromoProducts.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenario, opt => opt.MapFrom(x => savedScenario))
                    .ForMember(pTo => pTo.RollingScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.MapFrom(f => f.PromoProductPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore());
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<Promo> promoesRA = mapper.Map<List<Promo>>(promoes);
            Context.Set<Promo>().AddRange(promoesRA);
            Context.SaveChanges();
            foreach (var promoRA in promoesRA)
            {
                if (promoRA.PromoPriceIncrease != null)
                {
                    foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoRA.PromoPriceIncrease.PromoProductPriceIncreases) // костыль
                    {
                        PromoProduct promoProduct = promoRA.PromoProducts.FirstOrDefault(g => g.ZREP == promoProductPriceIncrease.ZREP);
                        promoProductPriceIncrease.PromoProductId = promoProduct.Id;
                    }
                }
            }
            Context.SaveChanges();
            return promoesRA;

        }
        public static List<Promo> CopyPromoesToSavedPromo(DatabaseContext Context, List<Promo> promoes, SavedPromo savedPromo, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.MapFrom(f => f.BTLPromoes.Where(g => !g.Disabled)))//filter
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
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.MapFrom(f => f.PromoSupportPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.MapFrom(f => f.PromoProductTrees.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => { opt.Condition(c => c.InOut == true); opt.MapFrom(f => f.IncrementalPromoes.Where(g => !g.Disabled)); })
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.MapFrom(f => f.PromoProducts.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedPromo, opt => opt.MapFrom(x => savedPromo))
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Archive))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore());
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<Promo> promoesHidden = mapper.Map<List<Promo>>(promoes);
            Context.Set<Promo>().AddRange(promoesHidden);
            Context.SaveChanges();
            foreach (var promoHidden in promoesHidden)
            {
                if (promoHidden.PromoPriceIncrease != null)
                {
                    foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoHidden.PromoPriceIncrease.PromoProductPriceIncreases) // костыль
                    {
                        PromoProduct promoProduct = promoHidden.PromoProducts.FirstOrDefault(g => g.ZREP == promoProductPriceIncrease.ZREP);
                        promoProductPriceIncrease.PromoProductId = promoProduct.Id;
                    }
                }
            }
            Context.SaveChanges();
            return promoesHidden;

        }
        public static List<Promo> CopyPromoesFromHiddenToRA(DatabaseContext Context, List<Promo> promoes, RollingScenario rollingScenario, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.MapFrom(f => f.BTLPromoes.Where(g => !g.Disabled)))//filter
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
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.MapFrom(f => f.PromoSupportPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.MapFrom(f => f.PromoProductTrees.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => { opt.Condition(c => c.InOut == true); opt.MapFrom(f => f.IncrementalPromoes.Where(g => !g.Disabled)); })
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.MapFrom(f => f.PromoProducts.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenario, opt => opt.MapFrom(f => rollingScenario))
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease))
                    .ForMember(pTo => pTo.CalculateML, opt => opt.MapFrom(f => true));
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RA))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore());
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<Promo> promoesRA = mapper.Map<List<Promo>>(promoes);
            var promoRAIds = promoesRA.Select(p => p.Id).ToList();
            Context.Set<Promo>().AddRange(promoesRA);
            Context.SaveChanges();
            foreach (var promoRA in promoesRA)
            {
                if (promoRA.PromoPriceIncrease != null)
                {
                    foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoRA.PromoPriceIncrease.PromoProductPriceIncreases) // костыль
                    {
                        PromoProduct promoProduct = promoRA.PromoProducts.FirstOrDefault(g => g.ZREP == promoProductPriceIncrease.ZREP);
                        promoProductPriceIncrease.PromoProductId = promoProduct.Id;
                    }
                }
            }
            Context.SaveChanges();
            var source = string.Format("{0} Scenario {1}", rollingScenario.ScenarioType.ToString(), rollingScenario.RSId);
            var mongoHelper = new MongoHelper<Guid>();
            mongoHelper.WriteScenarioPromoes(
                source,
                promoRAIds,
                Context.AuthManager.GetCurrentUser(),
                Context.AuthManager.GetCurrentRole(),
                OperationType.Created
            );
            return promoesRA;
        }
        public static CopyRAReturn CopyToPromoRA(DatabaseContext Context, List<Promo> promoes, int budgetYear, bool CheckedDate, PromoHelper.ClientDispatchDays clientDispatchDays, Guid draftPublish, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            CopyRAReturn copyRAReturn = new CopyRAReturn { Promos = new List<Promo>(), Errors = new List<string>() };
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.PrevousNumber, opt => opt.MapFrom(x => x.Number))
                    .ForMember(pTo => pTo.Number, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatusId, opt => opt.MapFrom(x => draftPublish))
                    .ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.BudgetYear, opt => opt.MapFrom(x => budgetYear))
                    .ForMember(pTo => pTo.StartDate, opt => opt.ConvertUsing(new DateTimeTypeConverter(CheckedDate)))
                    .ForMember(pTo => pTo.EndDate, opt => opt.ConvertUsing(new DateTimeTypeConverter(CheckedDate)))
                    .ForMember(pTo => pTo.DispatchesStart, opt => opt.Ignore())
                    .ForMember(pTo => pTo.DispatchesEnd, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.MapFrom(f => f.BTLPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanic, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MarsMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PlanInstoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Color, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RejectReason, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Event, opt => opt.Ignore())
                    .ForMember(pTo => pTo.MasterPromo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.MapFrom(f => f.PromoSupportPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.MapFrom(f => f.PromoProductTrees.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => { opt.Condition(c => c.InOut == true); opt.MapFrom(f => f.IncrementalPromoes.Where(g => !g.Disabled)); })
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.MapFrom(f => f.PromoProducts.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.SavedScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenarioId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.RollingScenario, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease))
                    .ForMember(pTo => pTo.UseActualTI, opt => opt.MapFrom(x => false))
                    .ForMember(pTo => pTo.UseActualCOGS, opt => opt.MapFrom(x => false))
                    .ForMember(pTo => pTo.ActualPromoXSites, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCatalogue, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoPOSMInClient, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCostProdXSites, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCostProdCatalogue, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCostProdPOSMInClient, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoBaselineLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreDiscount, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreShelfPrice, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalBaseTI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalBaseTI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalCOGS, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalCOGS, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoTotalCost, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalMAC, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalEarnings, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalEarnings, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetROIPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetUpliftPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualTIBasePercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualCOGSPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualCOGSTn, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoBaselineBaseTI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetBaseTI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoBaseTI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetNSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalMACLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalMACLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalEarningsLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalEarningsLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoROIPercentLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetROIPercentLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoTIShopper, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoTIMarketing, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoBranding, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoBTL, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCostProduction, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoCost, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoUpliftPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoLSVByCompensation, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoLSVSI, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoLSVSO, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoPostPromoEffectLSVW1, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoPostPromoEffectLSVW2, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoPostPromoEffectLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoROIPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalNSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoNetIncrementalNSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualPromoIncrementalMAC, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanicType, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualInStoreMechanic, opt => opt.Ignore());
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductBaselineLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPCQty, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductCaseQty, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductUOM, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductSellInPrice, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductShelfDiscount, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPCLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductUpliftPercent, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductIncrementalPCQty, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductIncrementalPCLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductIncrementalLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductLSV, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectQtyW1, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectQtyW2, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectQty, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductLSVByCompensation, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductBaselineCaseQty, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductQtySO, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectLSVW1, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ActualProductPostPromoEffectLSVW2, opt => opt.Ignore())
                    ;
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.Hidden))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.MapFrom(f => f.PromoProductPriceIncreases.Where(g => !g.Disabled)));
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductId, opt => opt.Ignore());
                //.AfterMap((src, dest) => dest.PromoProduct = dest.PromoPriceIncrease.Promo.PromoProducts.FirstOrDefault(g=>g.ZREP == dest.ZREP)); не работает не видит сущности EF6
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<Promo> promoesRA = mapper.Map<List<Promo>>(promoes);
            List<Promo> notCopyPromoes = new List<Promo>();
            OneLoadModel oneLoad = new OneLoadModel
            {
                ClientTrees = Context.Set<ClientTree>().Where(g => g.EndDate == null).ToList(),
                BrandTeches = Context.Set<BrandTech>().Where(g => !g.Disabled).ToList(),
                COGSs = Context.Set<COGS>().Where(x => !x.Disabled).ToList(),
                PlanCOGSTns = Context.Set<PlanCOGSTn>().Where(x => !x.Disabled).ToList(),
                ProductTrees = Context.Set<ProductTree>().Where(g => g.EndDate == null).ToList(),
                TradeInvestments = Context.Set<TradeInvestment>().Where(x => !x.Disabled).ToList(),
                Products = Context.Set<Product>().Where(g => !g.Disabled).ToList()
            };
            foreach (Promo promoRA in promoesRA)
            {
                DateTimeOffset startDate = (DateTimeOffset)(promoRA.StartDate);
                DateTimeOffset endDate = (DateTimeOffset)(promoRA.EndDate);
                if (clientDispatchDays.IsStartAdd)
                {
                    promoRA.DispatchesStart = startDate.AddDays(clientDispatchDays.StartDays);
                }
                else
                {
                    promoRA.DispatchesStart = startDate.AddDays(-clientDispatchDays.StartDays);
                }
                if (clientDispatchDays.IsEndAdd)
                {
                    promoRA.DispatchesEnd = endDate.AddDays(clientDispatchDays.EndDays);
                }
                else
                {
                    promoRA.DispatchesEnd = endDate.AddDays(-clientDispatchDays.EndDays);
                }
                if (promoRA.PromoProducts.Count != 0)
                {
                    try
                    {
                        //List<PromoProductTree> promoProductTrees = PromoHelper.AddProductTrees(promoRA.ProductTreeObjectIds, promoRA, out bool isSubrangeChanged, Context);
                        PromoHelper.CheckSupportInfo(promoRA, promoRA.PromoProductTrees.ToList(), oneLoad, Context);
                    }
                    catch (Exception ex)
                    {
                        copyRAReturn.Errors.Add("Promo:" + promoRA.PrevousNumber.ToString() + " - " + ex.Message);
                        notCopyPromoes.Add(promoRA);
                        continue;
                    }
                }
                else
                {
                    copyRAReturn.Errors.Add("Promo:" + promoRA.PrevousNumber.ToString() + " - " + " Not present products in promo");
                    notCopyPromoes.Add(promoRA);
                }
            }
            foreach (var item in notCopyPromoes)
            {
                promoesRA.Remove(item);
            }
            if (promoesRA.Count == 0)
            {
                copyRAReturn.Errors.Add("No promoes to copy");
                return copyRAReturn;
            }
            Context.Set<Promo>().AddRange(promoesRA);
            Context.SaveChanges();
            foreach (Promo promoRA in promoesRA)
            {
                if (promoRA.PromoPriceIncrease != null)
                {
                    List<PromoProductPriceIncrease> promoProductsPIdelete = new List<PromoProductPriceIncrease>();
                    foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoRA.PromoPriceIncrease.PromoProductPriceIncreases) // костыль
                    {
                        PromoProduct promoProduct = promoRA.PromoProducts.FirstOrDefault(g => g.ZREP == promoProductPriceIncrease.ZREP);
                        if (promoProduct != null)
                        {
                            promoProductPriceIncrease.PromoProductId = promoProduct.Id;
                        }
                        else
                        {
                            promoProductsPIdelete.Add(promoProductPriceIncrease);
                        }
                    }
                    Context.Set<PromoProductPriceIncrease>().RemoveRange(promoProductsPIdelete);
                }
            }
            Context.SaveChanges();
            copyRAReturn.Promos = promoesRA;
            return copyRAReturn;

        }
        public class DateTimeTypeConverter : IValueConverter<DateTimeOffset?, DateTimeOffset?>
        {
            public DateTimeTypeConverter(bool dayweek)
            {
                this.Dayweek = dayweek;
            }

            private bool Dayweek { get; set; }
            public DateTimeOffset? Convert(DateTimeOffset? source, ResolutionContext context)
            {
                DateTimeOffset addYaer = ((DateTimeOffset)source).AddYears(1);
                if (Dayweek)
                {
                    int dayWeekSource = (int)((DateTimeOffset)source).DayOfWeek;
                    int dayWeekaddYear = (int)addYaer.DayOfWeek;
                    if (dayWeekSource == dayWeekaddYear)
                    {
                        return addYaer;
                    }
                    else if (dayWeekSource > dayWeekaddYear)
                    {
                        if ((dayWeekSource - dayWeekaddYear) > 3)
                        {
                            return addYaer.AddDays(dayWeekSource - dayWeekaddYear - 7);
                        }
                        else
                        {
                            return addYaer.AddDays(dayWeekSource - dayWeekaddYear);
                        }
                    }
                    else if (dayWeekSource < dayWeekaddYear)
                    {
                        if ((dayWeekaddYear - dayWeekSource) < 3)
                        {
                            return addYaer.AddDays(dayWeekSource - dayWeekaddYear);
                        }
                        else
                        {
                            return addYaer.AddDays(dayWeekSource - dayWeekaddYear + 7);
                        }
                    }
                    return addYaer;
                }
                else
                {
                    return addYaer;
                }
            }
        }
    }
}
