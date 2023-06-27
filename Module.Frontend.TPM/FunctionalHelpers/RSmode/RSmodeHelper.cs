using AutoMapper;
using Module.Frontend.TPM.FunctionalHelpers.RSPeriod;
using Module.Persist.TPM.Model.Interfaces;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Module.Frontend.TPM.FunctionalHelpers.Scenario;

namespace Module.Frontend.TPM.FunctionalHelpers.RSmode
{
    public static class RSmodeHelper
    {
        public static void CreatePromoRS(DatabaseContext Context, Promo promo)
        {

        }
        public static Promo EditToPromoRS(DatabaseContext Context, Promo promo, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
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
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                //cfg.CreateMap<BTLPromo, BTLPromo>()
                //    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                //    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                //    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                //    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                //    .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.MapFrom(f => f.PromoProductPriceIncreases.Where(g => !g.Disabled)));
                cfg.CreateMap<PromoProductPriceIncrease, PromoProductPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ProductCorrectionPriceIncreases, opt => opt.MapFrom(f => f.ProductCorrectionPriceIncreases.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                    //.AfterMap((src, dest) => dest.PromoProduct = dest.PromoPriceIncrease.Promo.PromoProducts.FirstOrDefault(g=>g.ZREP == dest.ZREP)); не работает не видит сущности EF6
                cfg.CreateMap<PromoProductCorrectionPriceIncrease, PromoProductCorrectionPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProductPriceIncrease, opt => opt.Ignore());
            });
            var mapper = configuration.CreateMapper();
            Promo promoRS = mapper.Map<Promo>(promo);

            Context.Set<Promo>().Add(promoRS);
            Context.SaveChanges();
            if (promoRS.PromoPriceIncrease != null)
            {
                foreach (PromoProductPriceIncrease promoProductPriceIncrease in promoRS.PromoPriceIncrease.PromoProductPriceIncreases) // костыль
                {
                    PromoProduct promoProduct = promoRS.PromoProducts.FirstOrDefault(g => g.ZREP == promoProductPriceIncrease.ZREP);
                    promoProductPriceIncrease.PromoProductId = promoProduct.Id;
                }
            }            
            Context.SaveChanges();
            ScenarioHelper.CreateScenarioPeriod(promoRS, Context, TPMmode.RS);
            return promoRS;

        }
        public static List<Promo> EditToPromoRS(DatabaseContext Context, List<Promo> promoes, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
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
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                //cfg.CreateMap<BTLPromo, BTLPromo>()
                //    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                //    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                //    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                //    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                //    .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<Promo> promoesRS = mapper.Map<List<Promo>>(promoes);
            Context.Set<Promo>().AddRange(promoesRS);
            Context.SaveChanges();
            ScenarioHelper.CreateScenarioPeriod(promoesRS, Context, TPMmode.RS);
            return promoesRS;

        }
        public static Promo DeleteToPromoRS(DatabaseContext Context, Promo promo)
        {
            var disabled = true;
            var deleteddate = System.DateTime.Now;
            promo.DeletedDate = deleteddate;
            promo.Disabled = disabled;
            //foreach (BTLPromo item in promo.BTLPromoes)
            //{
            //    item.Disabled = disabled;
            //    item.DeletedDate = deleteddate;
            //}
            foreach (PromoSupportPromo item in promo.PromoSupportPromoes)
            {
                item.Disabled = disabled;
                item.DeletedDate = deleteddate;
            }
            foreach (PromoProductTree item in promo.PromoProductTrees)
            {
                item.Disabled = disabled;
                item.DeletedDate = deleteddate;
            }
            foreach (IncrementalPromo item in promo.IncrementalPromoes)
            {
                item.Disabled = disabled;
                item.DeletedDate = deleteddate;
            }
            foreach (PromoProduct item in promo.PromoProducts)
            {
                item.Disabled = disabled;
                item.DeletedDate = deleteddate;
                foreach (PromoProductsCorrection correction in item.PromoProductsCorrections)
                {
                    correction.Disabled = disabled;
                    correction.DeletedDate = deleteddate;
                }
            }
            Context.SaveChanges();
            return promo;

        }

        public static List<PromoSupportPromo> EditToPromoSupportPromoRS(DatabaseContext Context, List<PromoSupportPromo> promoSupportPromoes, bool disabled = false, DateTimeOffset? deleteddate = null)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    .ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore());
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
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
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.MapFrom(f => f.PromoProductTrees.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => { opt.Condition(c => c.InOut == true); opt.MapFrom(f => f.IncrementalPromoes.Where(g => !g.Disabled)); })
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.MapFrom(f => f.PromoProducts.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                //cfg.CreateMap<BTLPromo, BTLPromo>()
                //    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                //    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                //    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                //    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                //    .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<PromoSupportPromo> promoSupportPromoesRS = mapper.Map<List<PromoSupportPromo>>(promoSupportPromoes);
            Context.Set<PromoSupportPromo>().AddRange(promoSupportPromoesRS);
            Context.SaveChanges();
            ScenarioHelper.CreateScenarioPeriod(promoSupportPromoesRS.Select(g => g.Promo).ToList(), Context, TPMmode.RS);
            return promoSupportPromoesRS;

        }
        public static List<PromoProductsCorrection> EditToPromoProductsCorrectionRS(DatabaseContext Context, List<PromoProductsCorrection> promoProductsCorrections)
        {
            var ids = promoProductsCorrections.Select(f => f.PromoProduct.Promo).Select(g => g.Id);
            List<Promo> promos = Context.Set<Promo>()
                .Where(g => ids.Contains(g.Id))
                .Include(g => g.PromoSupportPromoes)
                .Include(g => g.PromoProductTrees)
                .Include(g => g.IncrementalPromoes)
                .Include(x => x.PromoProducts.Select(y => y.PromoProductsCorrections))
                .ToList();
            var promoRS = EditToPromoRS(Context, promos);
            return promoRS.SelectMany(g => g.PromoProducts).SelectMany(g => g.PromoProductsCorrections).ToList();

        }
        public static List<IncrementalPromo> EditToIncrementalPromoRS(DatabaseContext Context, List<IncrementalPromo> incrementalPromos)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IncrementalPromo, IncrementalPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore());
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.MapFrom(f => f.BTLPromoes.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
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
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.MapFrom(f => f.PromoPriceIncrease));
                //cfg.CreateMap<BTLPromo, BTLPromo>()
                //    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                //    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                //    .ForMember(pTo => pTo.BTL, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoSupportPromo, PromoSupportPromo>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.PromoSupport, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore());
                cfg.CreateMap<PromoProductTree, PromoProductTree>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore());
                cfg.CreateMap<PromoProduct, PromoProduct>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoId, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Product, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductsCorrections, opt => opt.MapFrom(f => f.PromoProductsCorrections.Where(g => !g.Disabled)))//filter
                    .ForMember(pTo => pTo.Plu, opt => opt.Ignore());
                cfg.CreateMap<PromoProductsCorrection, PromoProductsCorrection>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    .ForMember(pTo => pTo.TPMmode, opt => opt.MapFrom(x => TPMmode.RS))
                    .ForMember(pTo => pTo.PromoProduct, opt => opt.Ignore());
                cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                    .ForMember(pTo => pTo.Id, opt => opt.MapFrom(x => Guid.NewGuid()))
                    //.ForMember(pTo => pTo.Disabled, opt => opt.MapFrom(x => disabled))
                    //.ForMember(pTo => pTo.DeletedDate, opt => opt.MapFrom(x => deleteddate))
                    .ForMember(pTo => pTo.Promo, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            }
                );
            var mapper = configuration.CreateMapper();
            List<IncrementalPromo> incrementalPromosRS = mapper.Map<List<IncrementalPromo>>(incrementalPromos);

            Context.Set<IncrementalPromo>().AddRange(incrementalPromosRS);
            Context.SaveChanges();
            ScenarioHelper.CreateScenarioPeriod(incrementalPromosRS.Select(g => g.Promo).ToList(), Context, TPMmode.RS);
            return incrementalPromosRS;
        }
        public static string AddDisableRSPromoFromMLPeriod(List<Promo> promos, DatabaseContext Context)
        {
            DateTimeOffset? startPeriod = promos.Select(g => g.DispatchesStart).Min();
            DateTimeOffset? endPeriod = promos.Select(g => g.EndDate).Min();
            var client = promos.FirstOrDefault().ClientTreeId;
            List<Promo> promosToDeleteRS = Context.Set<Promo>().Where(g => g.ClientTreeId == client && g.DispatchesStart > startPeriod && g.EndDate < endPeriod && string.IsNullOrEmpty(g.MLPromoId)).ToList();
            string numbers = string.Join(",", promosToDeleteRS.Select(g=>g.Number).ToList());
            EditToPromoRS(Context, promosToDeleteRS, true, DateTimeOffset.Now);
            return numbers;
        }
    }
}
