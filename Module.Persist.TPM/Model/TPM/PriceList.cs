using Core.Data;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Model.TPM
{
    public class PriceList : IEntity<Guid>, IDeactivatable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }

        public bool Disabled { get; set; }

        [Index("Unique_PriceList", 1, IsUnique = true)]
        public DateTimeOffset? DeletedDate { get; set; }

        [Index("Unique_PriceList", 2, IsUnique = true)]
        public DateTimeOffset StartDate { get; set; }

        [Index("Unique_PriceList", 3, IsUnique = true)]
        public DateTimeOffset EndDate { get; set; }
                
        public DateTimeOffset? ModifiedDate { get; set; }

        public double Price { get; set; }

        [Index("Unique_PriceList", 4, IsUnique = true)]
        public int ClientTreeId { get; set; }

        [Index("Unique_PriceList", 5, IsUnique = true)]
        public Guid ProductId { get; set; }

        [Index("Unique_PriceList", 6, IsUnique = true)]
        public bool FuturePriceMarker { get; set; }

        public virtual ClientTree ClientTree { get; set; }
        public virtual Product Product { get; set; }

        public override string ToString()
        {
            var stringData =
              $"{nameof(StartDate)} = {StartDate}, " +
              $"{nameof(EndDate)} = {EndDate}, " +
              $"{nameof(Price)} = {Price}, " +
              $"Client hierarchy code = {ClientTree.ObjectId}, " +
              $"{nameof(Product.ZREP)} = {Product.ZREP}";

            return stringData;
        }
    }

    public class PriceListEqualityComparer : IEqualityComparer<PriceList>
    {
        public bool Equals(PriceList x, PriceList y)
        {
            return
                x.Disabled == y.Disabled &&
                x.DeletedDate == y.DeletedDate &&
                x.StartDate == y.StartDate &&
                x.EndDate == y.EndDate &&
                x.Price == y.Price &&
                x.ClientTreeId == y.ClientTreeId &&
                x.ProductId == y.ProductId &&
                x.FuturePriceMarker == y.FuturePriceMarker;
        }

        public class checkPriceListEqualityComparer : IEqualityComparer<PriceList>
        {
            public bool Equals(PriceList x, PriceList y)
            {
                return
                    x.DeletedDate == y.DeletedDate &&
                    x.StartDate == y.StartDate &&
                    x.ClientTreeId == y.ClientTreeId &&
                    x.ProductId == y.ProductId;
            }

            public int GetHashCode(PriceList obj)
            {
                return new { obj.DeletedDate, obj.StartDate, obj.ClientTreeId, obj.ProductId }.GetHashCode();
            }
        }

        public int GetHashCode(PriceList obj)
        {
            return new { obj.Disabled, obj.DeletedDate, obj.StartDate, obj.EndDate, obj.Price, obj.ClientTreeId, obj.ProductId }.GetHashCode();
        }
    }
}
