using Core.Data;

using Module.Host.TPM.Util.Model;

using System;
using System.Collections.Generic;

namespace Module.Host.TPM.Util.Interface
{
    public interface IDispatcherLogInfo<TType>
    {
        IList<ILogInfo> LogInfos { get; set;}
        void Add<T>(IEntity<Guid> entity, string message,string column, string data) where T : class,ILogInfo,new();

        TType GetLog<T>() where T : class, ILogInfo, new();

    }
}
