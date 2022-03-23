using Core.Data;
using System;

namespace Module.Persist.TPM.Model.TPM
{
    /// <summary>
    /// Создан для предотвращения ошибки, когда IsSplittable (bool) преобразуется в "+" и "-" (string)
    /// </summary>
    public class TechnologyExport : IEntity<Guid>, IDeactivatable
    {
        public Guid Id { get; set; }
        public bool Disabled { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public string Name { get; set; }
        public string Description_ru { get; set; }
        public string Tech_code { get; set; }
        public string SubBrand { get; set; }
        public string SubBrand_code { get; set; }
        public bool IsSplittable { get; set; }
        public string Splittable { get; set; }//используется для импорта Technology (Excel файл)
    }
}
