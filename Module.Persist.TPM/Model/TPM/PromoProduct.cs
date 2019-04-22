using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class PromoProduct : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public Guid PromoId { get; set; }
        public Guid ProductId { get; set; }

        [StringLength(255)]
        public string ZREP { get; set; }

        /// <summary>
        /// EAN код продукта
        /// </summary>
        [StringLength(255)]
        public string EAN { get; set; }

        /// <summary>
        /// Плановое количество в кейсах, расчитывается исходя из дат промо
        /// </summary>
        [Range(0, 10000000000)]
        public double? PlanProductQty { get; set; }

        /// <summary>
        /// Плановое количество в штуках, расчитывается исходя из дат промо
        /// </summary>
        [Range(0, 1000000000)]
        public int? PlanProductPCQty { get; set; }

        /// <summary>
        /// Плановая сумма продажи в кейсах
        /// </summary>
        [Range(0, 10000000000)]
        public double? PlanProductLSV { get; set; }

        /// <summary>
        /// Плановая сумма продажи в штуках
        /// </summary>
        [Range(0, 10000000000)]
        public double? PlanProductPCLSV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, 10000000000)]
        public double? PlanProductBaselineLSV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, 10000000000)]
        public double? PlanProductBaselineQty { get; set; }

        /// <summary>
        /// Базовая цена продажи
        /// </summary>
        [Range(0, 10000000000)]
        public double? ProductBaselinePrice { get; set; }

        /// <summary>
        /// Базовая цена продажи
        /// </summary>
        [Range(0, 10000000000)]
        public double? ProductBaselinePCPrice { get; set; }

        /// <summary>
        /// Значение планового аплифта
        /// </summary>
        public double? PlanProductUplift { get; set; }

        /// <summary>
        /// Фактическое количество в штуках
        /// </summary>
        [Range(0, 1000000000)]
        public int? ActualProductPCQty { get; set; }

        /// <summary>
        /// Фактическое количество в кейсах
        /// </summary>
        [Range(0, 10000000000)]
        public double? ActualProductQty { get; set; }

        /// <summary>
        /// Единица измерения (штуки или кейсы)
        /// </summary>
        public string ActualProductUOM { get; set; }

        /// <summary>
        /// Фактическая закупочная цена
        /// </summary>
        [Range(0, 1000000000)]
        public double? ActualProductSellInPrice { get; set; }

        /// <summary>
        /// Фактическая закупочная скидка
        /// </summary>
        public double? ActualProductSellInDiscount { get; set; }

        /// <summary>
        /// Цена на полке
        /// </summary>
        [Range(0, 1000000000)]
        public double? ActualProductShelfPrice { get; set; }

        /// <summary>
        /// Размер фактической скидки на полке
        /// </summary>
        public double? ActualProductShelfDiscount { get; set; }

        /// <summary>
        /// Общая сумма продажи
        /// </summary>
        [Range(0, 1000000000)]
        public double? ActualProductPCLSV { get; set; }

        /// <summary>
        /// Сколько продукт составляет от общей суммы промо
        /// </summary>
        public double? ActualPromoShare { get; set; }

        /// <summary>
        /// Значение аплифта по отношению к baseline
        /// </summary>
        public double? ActualProductUplift { get; set; }

        /// <summary>
        /// Увеличение продаж относительно плана (Qty) в штуках
        /// </summary>
        public double? ActualProductIncrementalPCQty { get; set; }

        /// <summary>
        /// Увеличение продаж относительно плана (LSV) в штуках
        /// </summary>
        public double? ActualProductIncrementalPCLSV { get; set; }

        /// <summary>
        /// Увеличение продаж относительно плана (LSV) в кейсах
        /// </summary>
        public double? ActualProductIncrementalLSV { get; set; }

        /// <summary>
        /// Плановое изменение продаж в первую неделю после проведения промо
        /// </summary>
        public double? PlanPostPromoEffectLSVW1 { get; set; }

        /// <summary>
        /// Плановое изменение продаж во вторую неделю после проведения промо
        /// </summary>
        public double? PlanPostPromoEffectLSVW2 { get; set; }

        /// <summary>
        /// Плановое изменение продаж в первую и вторую недели после проведения промо
        /// </summary>
        public double? PlanPostPromoEffectLSV { get; set; }

        /// <summary>
        /// Фактическое изменение продаж в первую неделю после проведения промо
        /// </summary>
        public double? ActualPostPromoEffectLSVW1 { get; set; }

        /// <summary>
        /// Фактическое изменение продаж во вторую неделю после проведения промо
        /// </summary>
        public double? ActualPostPromoEffectLSVW2 { get; set; }

        /// <summary>
        /// Фактическое изменение продаж в первую и вторую недели после проведения промо
        /// </summary>
        public double? ActualPostPromoEffectLSV { get; set; }

        /// <summary>
        /// Планируемое увеличение продаж относительно плана (Qty) в штуках  
        /// </summary>
        public double? PlanProductIncrementalQty { get; set; }

        /// <summary>
        /// Значение планового аплифта в процентах
        /// </summary>
        public double? PlanProductUpliftPercent { get; set; }

        /// <summary>
        /// Фактическая сумма продажи в кейсах
        /// </summary>
        public double? ActualProductLSV { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? ActualProductPostPromoEffectQty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? PlanProductPostPromoEffectQty { get; set; }

            
        /// <summary>
        /// Имя продукта на EN
        /// </summary>
        public string ProductEN { get; set; }

        public virtual Promo Promo { get; set; }
        public virtual Product Product  { get; set; }
    }
}
