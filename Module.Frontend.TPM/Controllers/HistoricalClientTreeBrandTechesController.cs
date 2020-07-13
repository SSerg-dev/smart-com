using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers
{

    public class HistoricalClientTreeBrandTechesController : ODataController
    {
        [Inject]
        public IHistoryReader HistoryReader { get; set; }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalClientTreeBrandTech> GetHistoricalClientTreeBrandTeches()
        {
            return HistoryReader.GetAll<HistoricalClientTreeBrandTech>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalClientTreeBrandTech> GetHistoricalClientTreeBrandTeches(Guid? Id)
        {
            return HistoryReader.GetAllById<HistoricalClientTreeBrandTech>(Id.ToString());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalClientTreeBrandTech> GetFilteredData(ODataQueryOptions<HistoricalClientTreeBrandTech> options)
        {
            var query = Enumerable.Empty<HistoricalClientTreeBrandTech>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                query = HistoryReader.GetAllById<HistoricalClientTreeBrandTech>(id.ToString());
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalClientTreeBrandTech>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalClientTreeBrandTech>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalClientTreeBrandTech>;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}



