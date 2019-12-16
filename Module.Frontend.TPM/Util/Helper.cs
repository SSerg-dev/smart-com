using Core.Dependency;
using Core.Settings;
using Frontend.Core.Extensions.Export;
using Looper.Core;
using Looper.Parameters;
using Module.Persist.TPM.CalculatePromoParametersModule;
using Module.Persist.TPM.Model.TPM;
using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;

namespace Module.Frontend.TPM.Util
{
    static class Helper
    {
        static public List<ClientTree> getClientTreeAllChildren(int parentId, DatabaseContext Context)
        {
            var tempClientTrees = Context.Set<ClientTree>().Where(x => !x.EndDate.HasValue && x.parentId == parentId).ToList();
            var resClientTrees = new List<ClientTree>(tempClientTrees);
            foreach (var clientTree in tempClientTrees)
            {
                resClientTrees.AddRange(getClientTreeAllChildren(clientTree.ObjectId, Context));
            }
            return resClientTrees;
        }
    }
}
