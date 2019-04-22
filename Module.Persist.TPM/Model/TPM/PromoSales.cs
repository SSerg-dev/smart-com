using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoSales : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? BrandTechId { get; set; }
        public Guid? PromoStatusId { get; set; }
        public Guid? MechanicId { get; set; }
        public Guid? MechanicTypeId { get; set; }
        public Guid? BudgetItemId { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset? DispatchesStart { get; set; }
        public DateTimeOffset? DispatchesEnd { get; set; }
        public int? Plan { get; set; }
        public int? Fact { get; set; }
        public int? MechanicDiscount { get; set; }

        public virtual Client Client { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual BrandTech BrandTech { get; set; }
        public virtual PromoStatus PromoStatus { get; set; }
        public virtual Mechanic Mechanic { get; set; }
        public virtual MechanicType MechanicType { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }
    }
}
