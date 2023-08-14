using Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ClientTree : IEntity<int>, ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index("CX_ObjDate", 1, IsUnique = true)]
        public int ObjectId { get; set; }
        public int parentId { get; set; }
        public int depth { get; set; }
        public string Type { get; set; }
        public string RetailTypeName { get; set; }
        public string Name { get; set; }
        public string FullPathName { get; set; }
        public bool? IsOnInvoice { get; set; }
        public string DMDGroup { get; set; }
        public DateTime StartDate { get; set; }
        [Index("CX_ObjDate", 2, IsUnique = true)]
        public DateTime? EndDate { get; set; }
        [StringLength(255)]
        public string GHierarchyCode { get; set; }
        [StringLength(255)]
        public string DemandCode { get; set; }
        public bool IsBaseClient { get; set; }

        //Dispatch start
        public bool? IsBeforeStart { get; set; }
        public int? DaysStart { get; set; }
        public bool? IsDaysStart { get; set; }

        //Dispatch end
        public bool? IsBeforeEnd { get; set; }
        public int? DaysEnd { get; set; }
        public bool? IsDaysEnd { get; set; }

        public double? DeviationCoefficient { get; set; }

        public string LogoFileName { get; set; }

        public double? DistrMarkUp { get; set; }

        public string SFAClientCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<AssortmentMatrix> AssortmentMatrices { get; set; }
        public ICollection<BudgetSubItemClientTree> BudgetSubItemClientTrees { get; set; }
        public ICollection<ClientTreeBrandTech> ClientTreeBrandTeches { get; set; }
        public ICollection<CompetitorPromo> CompetitorPromoes { get; set; }
        public ICollection<EventClientTree> EventClientTrees { get; set; }
        public ICollection<MechanicType> MechanicTypes { get; set; }
        public ICollection<NoneNego> NoneNegoes { get; set; }
        public ICollection<NonPromoSupport> NonPromoSupports { get; set; }
        public ICollection<Plu> Plus { get; set; }
        public ICollection<PriceList> PriceLists { get; set; }
        public ICollection<PromoSupport> PromoSupports { get; set; }
        public ICollection<RATIShopper> RATIShoppers { get; set; }
        public ICollection<TradeInvestment> TradeInvestments { get; set; }
        public ICollection<PlanPostPromoEffect> PlanPostPromoEffects { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
