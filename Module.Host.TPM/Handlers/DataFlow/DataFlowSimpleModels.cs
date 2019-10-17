using Core.Data;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Host.TPM.Handlers.DataFlow
{
    public class DataFlowModuleCollection
    {
        public PromoDataFlowModule PromoDataFlowModule { get; }
        public PromoProductDataFlowModule PromoProductDataFlowModule { get; }
        public ProductDataFlowModule ProductDataFlowModule { get; }
        public BaseLineDataFlowModule BaseLineDataFlowModule { get; }
        public AssortmentMatrixDataFlowModule AssortmentMatrixDataFlowModule { get; }
        public ClientTreeDataFlowModule ClientTreeDataFlowModule { get; }
        public ProductTreeDataFlowModule ProductTreeDataFlowModule { get; }
        public PromoProductTreeDataFlowModule PromoProductTreeDataFlowModule { get; }
        public IncrementalPromoDataFlowModule IncrementalPromoDataFlowModule { get; }
        public DataFlowModuleCollection(DatabaseContext databaseContext)
        {
            this.PromoDataFlowModule = new PromoDataFlowModule(databaseContext);
            this.PromoProductDataFlowModule = new PromoProductDataFlowModule(databaseContext);
            this.ProductDataFlowModule = new ProductDataFlowModule(databaseContext);
            this.BaseLineDataFlowModule = new BaseLineDataFlowModule(databaseContext);
            this.AssortmentMatrixDataFlowModule = new AssortmentMatrixDataFlowModule(databaseContext);
            this.ClientTreeDataFlowModule = new ClientTreeDataFlowModule(databaseContext);
            this.ProductTreeDataFlowModule = new ProductTreeDataFlowModule(databaseContext);
            this.PromoProductTreeDataFlowModule = new PromoProductTreeDataFlowModule(databaseContext);
            this.IncrementalPromoDataFlowModule = new IncrementalPromoDataFlowModule(databaseContext);
        }
    }

    public class DataFlowModule
    {
        protected DatabaseContext DatabaseContext { get; }
        protected DataFlowModule(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
        public abstract class DataFlowSimpleModel { }
    }

    // Promo
    public class PromoDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<PromoDataFlowSimpleModel> Collection { get; }
        public PromoDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<Promo>().AsNoTracking()
            .Select(x => new PromoDataFlowSimpleModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ClientTreeId = x.ClientTreeId,
                ClientTreeKeyId = x.ClientTreeKeyId,
                DispatchesStart = x.DispatchesStart,
                DispatchesEnd = x.DispatchesEnd,
                InOut = x.InOut,
                PromoStatusSystemName = x.PromoStatus.SystemName,
                Number = x.Number,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class PromoDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
            public DateTimeOffset? DispatchesStart { get; set; }
            public DateTimeOffset? DispatchesEnd { get; set; }
            public int? ClientTreeId { get; set; }
            public int? ClientTreeKeyId { get; set; }
            public int? Number { get; set; }
            public string PromoStatusSystemName { get; set; }
            public bool? InOut { get; set; }
            public bool Disabled { get; set; }
        }
    }

    // PromoProduct
    public class PromoProductDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<PromoProductDataFlowSimpleModel> Collection { get; }
        public PromoProductDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PromoProduct>().AsNoTracking()
            .Select(x => new PromoProductDataFlowSimpleModel
            {
                PromoId = x.PromoId,
                ProductId = x.ProductId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class PromoProductDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid? PromoId { get; set; }
            public Guid? ProductId { get; set; }
            public bool Disabled { get; set; }
        }
    }

    // Product
    public class ProductDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<ProductDataFlowSimpleModel> Collection { get; }
        public ProductDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<Product>().AsNoTracking()
            .Select(x => new ProductDataFlowSimpleModel
            {
            })
            .ToList();
        }
        public class ProductDataFlowSimpleModel : DataFlowSimpleModel
        {
        }
    }

    // BaseLine
    public class BaseLineDataFlowModule : DataFlowModule
    {
        public IEnumerable<BaseLineDataFlowSimpleModel> Collection { get; }
        public BaseLineDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<BaseLine>().AsNoTracking()
            .Select(x => new BaseLineDataFlowSimpleModel
            {
                Id = x.Id,
                StartDate = x.StartDate,
                ProductId = x.ProductId
            })
            .ToList();
        }
        public class BaseLineDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public Guid ProductId { get; set; }
            public int ClientTreeId { get; set; }
        }
    }

    // AssortmentMatrix
    public class AssortmentMatrixDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<AssortmentMatrixDataFlowSimpleModel> Collection { get; }
        public AssortmentMatrixDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<AssortmentMatrix>().AsNoTracking()
            .Select(x => new AssortmentMatrixDataFlowSimpleModel
            {
                Id = x.Id,
                Disabled = x.Disabled,
                ClientTreeId = x.ClientTreeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToList();
        }
        public class AssortmentMatrixDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public bool Disabled { get; set; }
            public int ClientTreeId { get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
        }
    }

    // ClientTree
    public class ClientTreeDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<ClientTreeDataFlowSimpleModel> Collection { get; }
        public ClientTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ClientTree>().AsNoTracking()
            .Select(x => new ClientTreeDataFlowSimpleModel
            {
                Id = x.Id,
                ObjectId = x.ObjectId,
                EndDate = x.EndDate,
                Type = x.Type,
                ParentId = x.parentId
            })
            .ToList();
        }
        public class ClientTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public DateTimeOffset? EndDate { get; set; }
            public int Id { get; set; }
            public int ObjectId { get; set; }
            public int ParentId { get; set; }
            public string Type { get; set; }
        }
    }

    // ProductTree
    public class ProductTreeDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<ProductTreeDataFlowSimpleModel> Collection { get; }
        public ProductTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<ProductTree>().AsNoTracking()
            .Select(x => new ProductTreeDataFlowSimpleModel
            {
                Id = x.Id,
                EndDate = x.EndDate
            })
            .ToList();
        }
        public class ProductTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public DateTimeOffset? EndDate { get; set; }
            public int Id { get; set; }
            public int ObjectId { get; set; }
        }
    }

    // PromoProductTree
    public class PromoProductTreeDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<PromoProductTreeDataFlowSimpleModel> Collection { get; }
        public PromoProductTreeDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<PromoProductTree>().AsNoTracking()
            .Select(x => new PromoProductTreeDataFlowSimpleModel
            {
                PromoId = x.PromoId,
                ProductTreeObjectId = x.ProductTreeObjectId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class PromoProductTreeDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid PromoId { get; set; }
            public int ProductTreeObjectId { get; set; }
            public bool Disabled { get; set; }
        }
    }

    // IncrementalPromo
    public class IncrementalPromoDataFlowModule : DataFlowModule
    {
        public IReadOnlyCollection<IncrementalPromoDataFlowSimpleModel> Collection { get; }
        public IncrementalPromoDataFlowModule(DatabaseContext databaseContext) 
            : base(databaseContext)
        {
            this.Collection = this.DatabaseContext.Set<IncrementalPromo>().AsNoTracking()
            .Select(x => new IncrementalPromoDataFlowSimpleModel
            {
                Id = x.Id,
                PromoId = x.PromoId,
                Disabled = x.Disabled
            })
            .ToList();
        }
        public class IncrementalPromoDataFlowSimpleModel : DataFlowSimpleModel
        {
            public Guid Id { get; set; }
            public Guid PromoId { get; set; }
            public bool Disabled { get; set; }
        }
    }
}
