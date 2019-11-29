﻿using Core.Import;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module.Persist.TPM.Model.TPM;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportPromoProductsCorrection : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoProduct), TerminalEntityType = typeof(Promo), LookupPropertyName = nameof(Promo.Number))]
        [Display(Name = nameof(Promo.Number))]
        public int PromoNumber { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [NavigationPropertyMap(LookupEntityType = typeof(PromoProduct), TerminalEntityType = typeof(Product), LookupPropertyName = nameof(Product.ZREP))]
        [Display(Name = nameof(Product.ZREP))]
        public string ProductZREP { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        public double PlanProductUpliftPercentCorrected { get; set; }
    }
}