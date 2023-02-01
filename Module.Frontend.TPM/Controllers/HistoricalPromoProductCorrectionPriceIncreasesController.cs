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
    public class HistoricalPromoProductCorrectionPriceIncreasesController : ODataController
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
            MaxTop = 1024,
            MaxExpansionDepth = 3)]
        public IQueryable<HistoricalPromoProductCorrectionPriceIncrease> GetHistoricalPromoProductCorrectionPriceIncreases()
        {
            return HistoryReader.GetAll<HistoricalPromoProductCorrectionPriceIncrease>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024,
            MaxExpansionDepth = 3)]
        public IQueryable<HistoricalPromoProductCorrectionPriceIncrease> GetHistoricalPromoProductCorrectionPriceIncreases(Guid? Id)
        {
            return HistoryReader.GetAllById<HistoricalPromoProductCorrectionPriceIncrease>(Id.ToString());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalPromoProductCorrectionPriceIncrease> GetFilteredData(ODataQueryOptions<HistoricalPromoProductCorrectionPriceIncrease> options)
        {
            var query = Enumerable.Empty<HistoricalPromoProductCorrectionPriceIncrease>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                query = HistoryReader.GetAllById<HistoricalPromoProductCorrectionPriceIncrease>(id.ToString());
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalPromoProductCorrectionPriceIncrease>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalPromoProductCorrectionPriceIncrease>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalPromoProductCorrectionPriceIncrease>;
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
