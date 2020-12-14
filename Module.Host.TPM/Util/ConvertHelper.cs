using Castle.Components.DictionaryAdapter;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Util
{
    public static class ConvertHelper
    {
        public static IQueryable Convert(List<object> ids, Type from, Type to, DatabaseContext context) 
        {
            IQueryable query = null;
            if (from.Name.Equals(typeof(Promo).Name) && to.Name.Equals(typeof(ActualLSV).Name))
            {
                List<Guid> tmp = ids.Select(q => (Guid)q).ToList();
                query = context.Set<Promo>().Where(q => tmp.Contains(q.Id))
                    .Select(n => new ActualLSV
                    {
                        Id = n.Id,
                        Number = n.Number,
                        ClientHierarchy = n.ClientHierarchy,
                        Name = n.Name,
                        BrandTech = n.BrandTech.BrandsegTechsub,
                        Event = n.Event.Name,
                        Mechanic = n.Mechanic,
                        MechanicIA = n.MechanicIA,
                        StartDate = n.StartDate,
                        MarsStartDate = n.MarsStartDate,
                        EndDate = n.EndDate,
                        MarsEndDate = n.MarsEndDate,
                        DispatchesStart = n.DispatchesStart,
                        MarsDispatchesStart = n.MarsDispatchesStart,
                        DispatchesEnd = n.DispatchesEnd,
                        MarsDispatchesEnd = n.MarsDispatchesEnd,
                        Status = n.PromoStatus.Name,
                        ActualInStoreDiscount = n.ActualInStoreDiscount,
                        PlanPromoUpliftPercent = n.PlanPromoUpliftPercent,
                        ActualPromoUpliftPercent = n.ActualPromoUpliftPercent,
                        PlanPromoBaselineLSV = n.PlanPromoBaselineLSV,
                        ActualPromoBaselineLSV = n.ActualPromoBaselineLSV,
                        PlanPromoIncrementalLSV = n.PlanPromoIncrementalLSV,
                        ActualPromoIncrementalLSV = n.ActualPromoIncrementalLSV,
                        PlanPromoLSV = n.PlanPromoLSV,
                        ActualPromoLSVByCompensation = n.ActualPromoLSVByCompensation,
                        ActualPromoLSV = n.ActualPromoLSV,
                        ActualPromoLSVSI = n.ActualPromoLSVSI,
                        ActualPromoLSVSO = n.ActualPromoLSVSO,
                        PlanPromoPostPromoEffectLSVW1 = n.PlanPromoPostPromoEffectLSVW1,
                        ActualPromoPostPromoEffectLSVW1 = n.ActualPromoPostPromoEffectLSVW1,
                        PlanPromoPostPromoEffectLSVW2 = n.PlanPromoPostPromoEffectLSVW2,
                        ActualPromoPostPromoEffectLSVW2 = n.ActualPromoPostPromoEffectLSVW2,
                        PlanPromoPostPromoEffectLSV = n.PlanPromoPostPromoEffectLSV,
                        ActualPromoPostPromoEffectLSV = n.ActualPromoPostPromoEffectLSV,
                        InOut = n.InOut,
                        IsOnInvoice = n.IsOnInvoice
                    });
            }

            return query;
        }
    }
}
