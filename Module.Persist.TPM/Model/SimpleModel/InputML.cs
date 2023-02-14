using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.SimpleModel
{
    public class InputML
    {
        [Display(Name = "Promo ID")]
        public int PromoId { get; set; }
        [Display(Name = "PPG")]
        public string PPG { get; set; }
        [Display(Name = "Format")]
        public string Format { get; set; }
        [Display(Name = "Client code")]
        public int ClientCode { get; set; }
        [Display(Name = "ZREP")]
        public int ZREP { get; set; }
        [Display(Name = "StartDate")]
        public DateTimeOffset StartDate { get; set; }
        [Display(Name = "EndDate")]
        public DateTimeOffset EndDate { get; set; }
        [Display(Name = "Mechanic(Mars)")]
        public string MechanicMars { get; set; }
        [Display(Name = "Discount(Mars)")]
        public double DiscountMars { get; set; }
        [Display(Name = "Mech(Instore)")]
        public string MechInstore { get; set; }
        [Display(Name = "Instore Discount")]
        public double InstoreDiscount { get; set; }
        [Display(Name = "Planned Uplift")]
        public double PlannedUplift { get; set; }
        [Display(Name = "PlanInStore Shelf Price")]
        public double PlanInStoreShelfPrice { get; set; }
        [Display(Name = "Type")]
        public string TypeML { get; set; }
    }
}
