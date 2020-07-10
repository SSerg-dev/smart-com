using Module.Host.TPM.Util.Interface;
using Module.Host.TPM.Util.Model.Base;

namespace Module.Host.TPM.Util.Model
{
    public class ErrorLogInfo : LogInfo, IErrorLogger
    {
        public ErrorLogInfo()
            : base()
        { }
    }
}
