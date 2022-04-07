using Core.Import;
using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Persist.TPM.Model.Import
{
    public class ImportTechnology : BaseImportEntity
    {
        [ImportCSVColumn(ColumnNumber = 0)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [ImportCSVColumn(ColumnNumber = 1)]
        [Display(Name = "Technology RU")]
        public string Description_ru { get; set; }

        [ImportCSVColumn(ColumnNumber = 2)]
        [Display(Name = "Tech Code")]
        public string Tech_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 3)]
        [Display(Name = "Sub Brand Code")]
        public string SubBrand { get; set; }

        [ImportCSVColumn(ColumnNumber = 4)]
        [Display(Name = "Sub Brand Code")]
        public string SubBrand_code { get; set; }

        [ImportCSVColumn(ColumnNumber = 5)]
        [Display(Name = "Splittable")]
        public string Splittable
        {
            set 
            {
                if (value == "+")
                {
                    IsSplittable = true;
                }
                else if(value == "-")
                {
                    IsSplittable = false;
                }
                else
                {
                    throw new Exception("Splittable value is not correct");
                }
            }
            get { return null; }
        }

        public bool IsSplittable;
    }
}
