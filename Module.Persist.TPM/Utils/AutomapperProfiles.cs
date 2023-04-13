using AutoMapper;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Utils
{
    public static class AutomapperProfiles
    {
        public static Promo PromoCopy(Promo promo)
        {
            var config = PromoMapper();
            var mapper = config.CreateMapper();
            Promo promoCopy = mapper.Map<Promo>(promo);
            return promoCopy;
        }
        public static Promo PromoCopy(Promo promo, Promo proxy)
        {
            var config = PromoMapper2();
            var mapper = config.CreateMapper();
            Promo promoCopy = mapper.Map(promo, proxy);
            return promoCopy;
        }
        private static MapperConfiguration PromoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Promo, Promo>()
                    .ForMember(pTo => pTo.BTLPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
                    .ForMember(pTo => pTo.BrandTech, opt => opt.Ignore())
                    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
                    //.ForMember(pTo => pTo.MarsMechanic, opt => opt.Ignore())
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
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Condition(c => c.InOut == false))
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore());
                cfg.CreateMap<PromoStatus, PromoStatus>()
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore());
                cfg.CreateMap<Mechanic, Mechanic>()
                    .ForMember(pTo => pTo.NoneNegoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore());
            });
            return config;
        }
        private static MapperConfiguration PromoMapper2()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Promo, Promo>()
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
                    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
                    .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Condition(c => c.InOut == false))
                    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
                    .ForMember(pTo => pTo.Number, opt => opt.Ignore())
                    .ForMember(pTo => pTo.PromoPriceIncrease, opt => opt.Ignore());
                //cfg.CreateMap<PromoStatus, PromoStatus>()
                //    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore());
                //cfg.CreateMap<Mechanic, Mechanic>()
                //    .ForMember(pTo => pTo.NoneNegoes, opt => opt.Ignore())
                //    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore());
                //cfg.CreateMap<PromoPriceIncrease, PromoPriceIncrease>()
                //    .ForMember(pTo => pTo.PromoProductPriceIncreases, opt => opt.Ignore());
            });
            return config;
        }
    }
}
