using Core.Data;

using System;
using System.Collections.Generic;

namespace Module.Host.TPM.Util.Model
{
    public interface ILogInfo
    {
        string Column { get; set; }
        string Message { get; set; }
        List<Tuple<IEntity<Guid>,string>> Data { get; set;}
        void Add(IEntity<Guid> entity, string message, string column, string data);
        void AddData(IEntity<Guid> entity, string data);
        List<Tuple<IEntity<Guid>,string>> BuildToRecord();

        string Build();
    }
}
