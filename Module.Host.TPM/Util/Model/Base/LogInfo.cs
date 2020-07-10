using Core.Data;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Host.TPM.Util.Model.Base
{
    public abstract class LogInfo : ILogInfo
    {
        public string Column { get; set; }
        public string Message { get; set; }
        public List<Tuple<IEntity<Guid>, string>> Data { get; set; }

        public LogInfo()
        {
            Data = new List<Tuple<IEntity<Guid>, string>>();
        }

        public string Build()
        {
            return Message
                + System.Environment.NewLine
                + Column
                + System.Environment.NewLine
                + string.Join(System.Environment.NewLine, Data.Select(i => i.Item2).Distinct());
        }

        public void Add(IEntity<Guid> entity, string message, string column, string data)
        {
            Column = column;
            Message = message;
            Data.Add(new Tuple<IEntity<Guid>, string>(entity, data));
        }

        public List<Tuple<IEntity<Guid>, string>> BuildToRecord()
        {
            return Data.Select(i => new Tuple<IEntity<Guid>, string>(i.Item1, Message + " " + i.Item2)).ToList();
        }

        public void AddData(IEntity<Guid> entity, string data)
        {
            Data.Add(new Tuple<IEntity<Guid>, string>(entity, data));
        }
    }
}
