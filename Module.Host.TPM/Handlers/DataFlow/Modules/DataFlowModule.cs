using Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.DataFlow.Modules
{
    public abstract class DataFlowModule
    {
        protected DatabaseContext DatabaseContext { get; }
        protected DataFlowModule(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
        public abstract class DataFlowSimpleModel { }
    }
}
