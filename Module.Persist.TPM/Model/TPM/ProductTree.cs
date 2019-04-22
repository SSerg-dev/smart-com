using Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ProductTree : ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index("CX_ObjDate", 1, IsUnique = true)]
        public int ObjectId { get; set; }
        public int parentId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? TechnologyId { get; set; }
        public string Type { get; set; }
        public int depth { get; set; }
        public string Name { get; set; }
        public string FullPathName { get; set; }
        public string Abbreviation { get; set; }
        public DateTime StartDate { get; set; }
        [Index("CX_ObjDate", 2, IsUnique = true)]
        public DateTime? EndDate { get; set; }
        public string Filter { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
