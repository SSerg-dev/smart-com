using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Frontend.TPM.Model {
    public static class ProductFilterUtility {

        public static class ProductFilterName {
            public const string Brand = "Brand";
            public const string BrandTech = "BrandTech";
            public const string ZREP = "ZREP";
            public const string Subrange = "Subrange";
            public const string Category = "Category";
            public const string Segment = "Segment";
            public const string Technology = "Technology";
            public const string TechHighLevel = "TechHighLevel";
            public const string Program = "Program";
            public const string Format = "Format";
            public const string AgeGroup = "AgeGroup";
            public const string Variety = "Variety";
        }

        public static List<Guid> GetProductIds(DatabaseContext context, FilterContainer filter) {
            if (filter.filter != null) {
                IQueryable<Product> query = context.Set<Product>().Where(p => !p.Disabled);
                foreach (Filter f in filter.filter) {
                    List<Guid> ids = filter.GetFilterValue(f.fieldName);
                    //if (f.fieldName == ProductFilterName.Brand) {
                    //    query = ApplyBrandFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.BrandTech) {
                    //    query = ApplyBrandTechFilter(query, ids);
                    //} else if(f.fieldName == ProductFilterName.SKU) {
                    //    query = ApplyIdFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Subrange) {
                    //    query = ApplySubrangeFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Category) {
                    //    query = ApplyCategoryFilter(query, ids);
                    //} else if(f.fieldName == ProductFilterName.Segment) {
                    //    query = ApplySegmentFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Technology) {
                    //    query = ApplyTechnologyFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.TechHighLevel) {
                    //    query = ApplyTechHighLevelFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Program) {
                    //    query = ApplyProgramFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Format) {
                    //    query = ApplyFormatFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.AgeGroup) {
                    //    query = ApplyAgeGroupFilter(query, ids);
                    //} else if (f.fieldName == ProductFilterName.Variety) {
                    //    query = ApplyVarietyFilter(query, ids);
                    //}
                }
                return query.Select(p => p.Id).ToList();
            } else {
                return new List<Guid>();
            }
        }

        //public static void UpdatePromoProductLinks(DatabaseContext context, Promo promo, FilterContainer filter, bool needClean) {
        //    if (filter != null) {
        //        List<Guid> productIds = GetProductIds(context, filter);
        //        // clean links
        //        if (needClean) {
        //            CleanPromoProductLinks(context, promo.Id);
        //        }
        //        // create links
        //        CreatePromoProductLinks(context, promo, productIds);
        //    }
        //}

        //private static void CleanPromoProductLinks(DatabaseContext context, Guid promoId) {
        //    IEnumerable<PromoProduct> links = context.Set<PromoProduct>().Where(p => p.PromoId == promoId);
        //    context.Set<PromoProduct>().RemoveRange(links);
        //}

        //private static void CreatePromoProductLinks(DatabaseContext context, Promo promo, List<Guid> productIds) {
        //    if (productIds.Any()) {
        //        IEnumerable<PromoProduct> links = productIds.Select(p => new PromoProduct() {
        //            ProductId = p,
        //            Promo = promo
        //        });
        //        context.Set<PromoProduct>().AddRange(links);
        //    }
        //}

        public static void UpdateBrand(Promo model, FilterContainer productFilter) {
            if (productFilter != null) {
                List<Guid> ids = productFilter.GetFilterValue(ProductFilterName.Brand);
                if (ids != null && ids.Any()) {
                    Guid id = ids.First();
                    model.BrandId = ids.First();
                } else {
                    model.BrandId = null;
                }
            }
        }
        public static void UpdateBrandTech(Promo model, FilterContainer productFilter) {
            if (productFilter != null) {
                List<Guid> ids = productFilter.GetFilterValue(ProductFilterName.BrandTech);
                if (ids != null && ids.Any()) {
                    Guid id = ids.First();
                    model.BrandTechId = id;
                } else {
                    model.BrandTechId = null;
                }
            }
        }

        #region Product Filter Apply
        //private static IQueryable<Product> ApplyBrandFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.BrandId.HasValue || ids.Contains(p.BrandId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyBrandTechFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.BrandTechId.HasValue || ids.Contains(p.BrandTechId.Value));
        //    } else {
        //        return query;
        //    }
        //}

        private static IQueryable<Product> ApplyIdFilter(IQueryable<Product> query, List<Guid> ids) {
            if (ids != null && ids.Any()) {
                return query.Where(p => ids.Contains(p.Id));
            } else {
                return query;
            }
        }

        //private static IQueryable<Product> ApplySubrangeFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.SubrangeId.HasValue || ids.Contains(p.SubrangeId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyCategoryFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.CategoryId.HasValue || ids.Contains(p.CategoryId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplySegmentFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.SegmentId.HasValue || ids.Contains(p.SegmentId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyTechnologyFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.TechnologyId.HasValue || ids.Contains(p.TechnologyId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyTechHighLevelFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.TechHighLevelId.HasValue || ids.Contains(p.TechHighLevelId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyProgramFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.ProgramId.HasValue || ids.Contains(p.ProgramId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyFormatFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.FormatId.HasValue || ids.Contains(p.FormatId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyAgeGroupFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.AgeGroupId.HasValue || ids.Contains(p.AgeGroupId.Value));
        //    } else {
        //        return query;
        //    }
        //}
        //private static IQueryable<Product> ApplyVarietyFilter(IQueryable<Product> query, List<Guid> ids) {
        //    if (ids != null && ids.Any()) {
        //        return query.Where(p => !p.VarietyId.HasValue || ids.Contains(p.VarietyId.Value));
        //    } else {
        //        return query;
        //    }
        //}

        #endregion
    }
}
