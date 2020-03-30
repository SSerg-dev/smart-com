using Core.History;
using Frontend.Core.Controllers.Base;
using Module.Frontend.TPM.Util;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {

    public class HistoricalPromoesController : ODataController {
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
        public IQueryable<HistoricalPromo> GetHistoricalPromoes(Guid? promoIdHistory)
        {
            return HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());// HistoryReader.GetAll<HistoricalPromo>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalPromo> GetHistoricalPromoes(Guid? promoIdHistory, bool onlyCreator)
        {
            var result = HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());// HistoryReader.GetAll<HistoricalPromo>();
            result = result.Where(x => x.CreatorLogin != null);
            return result;
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalPromo> GetFilteredData(ODataQueryOptions<HistoricalPromo> options)
        {
            var query = Enumerable.Empty<HistoricalPromo>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "onlyCreator");
            if (isArgumentExists)
            {
                Guid? promoIdHistory = Helper.GetValueIfExists<Guid?>(bodyText, "promoIdHistory");
                query = HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());// HistoryReader.GetAll<HistoricalPromo>();
                query = query.Where(x => x.CreatorLogin != null);
            }
            else
            {
                Guid? promoIdHistory = Helper.GetValueIfExists<Guid?>(bodyText, "promoIdHistory");
                query = HistoryReader.GetById<HistoricalPromo>(promoIdHistory.ToString());
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalPromo>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalPromo>;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
