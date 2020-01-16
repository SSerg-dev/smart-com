using Core.History;
using Module.Persist.TPM.Model.History;
using Ninject;
using System;
using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Module.Frontend.TPM.Controllers {

    public class HistoricalStoreTypesController : ODataController {
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
        public IQueryable<HistoricalStoreType> GetHistoricalStoreTypes() {
            return HistoryReader.GetAll<HistoricalStoreType>();
        }

        [ClaimsAuthorize]
        [EnableQuery(
            MaxNodeCount = int.MaxValue,
            EnsureStableOrdering = false,
            HandleNullPropagation = HandleNullPropagationOption.False,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = false,
            MaxTop = 1024)]
        public IQueryable<HistoricalStoreType> GetHistoricalStoreTypes(Guid? Id)
        {
            return HistoryReader.GetAllById<HistoricalStoreType>(Id.ToString());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                HistoryReader.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}



