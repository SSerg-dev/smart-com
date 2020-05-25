using Core.Data;
using Module.Persist.TPM.Model.TPM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Export;

namespace Module.Persist.TPM.Utils.ExportCSVModelBuilders
{
    class IncrementalToApolloModelBuilder : IExportModelBuilder
    {
        public bool Build(IEntity<Guid> source, out object model, ref IList<string> errors)
        {
            bool result = true;
            model = new PromoProductDifference();
            try
            {
                model = source;
            }
            catch (ApplicationException e)
            {
                result = false;
                errors.Add(String.Format("Error has occurred when building data from export model: {0}", e.Message));
            }
            catch (Exception e)
            {
                result = false;
                errors.Add(String.Format("Error has occurred when building data from export model: {0}", e.ToString()));
            }
            return result;
        }
    }
}
