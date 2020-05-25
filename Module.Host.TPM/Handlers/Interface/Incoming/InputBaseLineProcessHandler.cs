using System;
using Interfaces.Core.Common;
using Looper.Parameters;
using Looper.Core;
using Module.Host.TPM.Actions.Interface.Incoming;

namespace Module.Host.TPM.Handlers.Interface.Incoming
{
    public class InputBaseLineProcessHandler : BaseProcessHandler
    {
        protected override IAction GetProcessorAction(HandlerInfo info, Guid interfaceId, FileBufferModel fileBuffer)
        {
            return new InputBaseLineCSVProcessorAction(info, interfaceId, fileBuffer);
        }
    }
}
