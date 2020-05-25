using Interfaces.Core.Common;
using Looper.Core;
using Looper.Parameters;
using Module.Host.TPM.Actions.Interface.Incoming;
using Module.Host.TPM.Actions.Interface.Outcoming;
using Module.Host.TPM.Handlers.Interface.Incoming;
using ProcessingHost.Handlers.Interface.Outcoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Handlers.Interface.Outcoming
{
    class OutputIncrementalProcessHandler : BaseProcessHandler
    {
        protected override IAction GetProcessorAction(HandlerInfo info, Guid interfaceId, FileBufferModel fileBuffer)
        {
            return new OutputIncrementalProcessAction(interfaceId, info.HandlerId, null);
        }
    }
}
