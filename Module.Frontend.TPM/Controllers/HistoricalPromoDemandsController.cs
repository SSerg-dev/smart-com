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

namespace Module.Frontend.TPM.Controllers {

    public class HistoricalPromoDemandsController : ODataController {
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
        public IQueryable<HistoricalPromoDemand> GetHistoricalPromoDemands() {
            return HistoryReader.GetAll<HistoricalPromoDemand>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalPromoDemand> GetHistoricalPromoDemands(Guid? Id)
        {
            return HistoryReader.GetAllById<HistoricalPromoDemand>(Id.ToString());
        }

        [ClaimsAuthorize]
        [HttpPost]
        public IQueryable<HistoricalPromoDemand> GetFilteredData(ODataQueryOptions<HistoricalPromoDemand> options)
        {
            var query = Enumerable.Empty<HistoricalPromoDemand>().AsQueryable();
            string bodyText = Helper.GetRequestBody(HttpContext.Current.Request);

            bool isArgumentExists = Helper.IsValueExists(bodyText, "Id");
            if (isArgumentExists)
            {
                Guid? id = Helper.GetValueIfExists<Guid?>(bodyText, "Id");
                query = HistoryReader.GetAllById<HistoricalPromoDemand>(id.ToString());
            }
            else
            {
                query = HistoryReader.GetAll<HistoricalPromoDemand>();
            }

            var querySettings = new ODataQuerySettings
            {
                EnsureStableOrdering = false,
                HandleNullPropagation = HandleNullPropagationOption.False,
                EnableConstantParameterization = false,
            };

            var optionsPost = new ODataQueryOptionsPost<HistoricalPromoDemand>(options.Context, Request, HttpContext.Current.Request);
            return optionsPost.ApplyTo(query, querySettings) as IQueryable<HistoricalPromoDemand>;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}



