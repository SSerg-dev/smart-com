using AutoMapper;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.FunctionalHelpers.RSmode
{
    public static class RSmodeHelper
    {
        public static void CreatePromoRS(DatabaseContext Context, Promo promo)
        {
            
        }
        public static Promo EditToPromoRS(DatabaseContext Context, Promo promo)
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<Promo, Promo>()
            //    .ForMember(pTo => pTo.Brand, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.Technology, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.BrandTech, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.ClientTree, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoStatus, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.MarsMechanic, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PlanInstoreMechanic, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.MarsMechanicType, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PlanInstoreMechanicType, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoTypes, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.Color, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.RejectReason, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.Event, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.ActualInStoreMechanic, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.ActualInStoreMechanicType, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.MasterPromo, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoUpliftFailIncidents, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoSupportPromoes, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoStatusChanges, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoProductTrees, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PreviousDayIncrementals, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.IncrementalPromoes, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.PromoProducts, opt => opt.Ignore())
            //    .ForMember(pTo => pTo.Promoes, opt => opt.Ignore())
            //    .MaxDepth(5);
            //});
            //promo.Brand = null;
            //promo.Technology = null;
            //promo.BrandTech = null;
            //promo.ClientTree = null;
            //promo.PromoStatus = null;
            //promo.MarsMechanic = null;
            //promo.PlanInstoreMechanic = null;
            //promo.MarsMechanicType = null;
            //promo.PlanInstoreMechanicType = null;
            //promo.PromoTypes = null;
            //promo.Color = null;
            //promo.RejectReason = null;
            //promo.Event = null;
            //promo.ActualInStoreMechanic = null;
            //promo.ActualInStoreMechanicType = null;
            //promo.MasterPromo = null;
            //promo.PromoUpliftFailIncidents = null;
            //promo.PromoSupportPromoes = null;
            //promo.PromoStatusChanges = null;
            //promo.PromoProductTrees = null;
            //promo.PreviousDayIncrementals = null;
            //promo.IncrementalPromoes = null;
            //promo.PromoProducts = null;
            //promo.Promoes = null;
            PromoStatus promoStatus = Mapper.Map<PromoStatus>(promo.PromoStatus);
            Promo promoRS = Mapper.Map<Promo>(promo);
            return promoRS;

        }
    }
}
