using AutoMapper;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.History;
using Module.Persist.TPM.MongoDB;

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
    }
}
