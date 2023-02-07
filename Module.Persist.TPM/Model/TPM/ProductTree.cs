using Core.Data;
using Module.Persist.TPM.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Persist.TPM.Model.TPM
{
    public class ProductTree : IEntity<int>, ICloneable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Index("CX_ObjDate", 1, IsUnique = true)]
        public int ObjectId { get; set; }
        public int parentId { get; set; }
        public Guid? BrandId { get; set; } //чего он тут одинокий?

        public string Type { get; set; }
        public int depth { get; set; }
        public string Name { get; set; }
        public string Description_ru { get; set; }
        public string FullPathName { get; set; }
        public string Abbreviation { get; set; }
        public DateTime StartDate { get; set; }
        [Index("CX_ObjDate", 2, IsUnique = true)]
        public DateTime? EndDate { get; set; }
        public string Filter { get; set; }
        public int? NodePriority { get; set; }

        public string LogoFileName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SpecialNotKeyProperty]
        public string FilterQuery { get; set; }

        public Guid? TechnologyId { get; set; }
        public Technology Technology { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<NoneNego> NoneNegoes { get; set; }
        public object Clone()
        {
            var clonedProductTree = new ProductTree
            {
                Id = this.Id,
                ObjectId = this.ObjectId,
                parentId = this.parentId,
                BrandId = this.BrandId,
                TechnologyId = this.TechnologyId,
                Technology = this.Technology,
                Type = this.Type,
                depth = this.depth,
                Name = this.Name,
                Description_ru = this.Description_ru,
                FullPathName = this.FullPathName,
                Abbreviation = this.Abbreviation,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Filter = this.Filter,
                NodePriority = this.NodePriority,
                LogoFileName = this.LogoFileName,
                FilterQuery = this.FilterQuery
            };
            return clonedProductTree;
        }
    }
}
