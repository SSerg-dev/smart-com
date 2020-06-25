using Core.Data;
using Module.Host.TPM.Util.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Util.Model
{
    public class WarningLogInfo:ILogInfo, IWarningLogger
    {
        public string Column { get; set; }
        public string Message { get; set; }
        public List<Tuple<IEntity<Guid>, string>> Data { get; set; }
       

        public WarningLogInfo()
        {
            Data = new List<Tuple<IEntity<Guid>, string>>();
        }

        public string Build()
        {
            string buffer = Message + System.Environment.NewLine + Column + System.Environment.NewLine;
            foreach (var item in Data)
            {
                buffer = buffer + item.Item2 + System.Environment.NewLine;
            }
            return buffer;
        }

        public void Add(IEntity<Guid> entity, string message, string column, string data)
        {
            Column = column;
            Message = message;
            Data.Add(new Tuple<IEntity<Guid>, string>(entity,data)); 
        }
        public void AddData(IEntity<Guid> entity, string data)
        {
            Data.Add(new Tuple<IEntity<Guid>, string>(entity, data));

        }
        public List<Tuple<IEntity<Guid>, string>> BuildToRecord()
        {
            List<Tuple<IEntity<Guid>, string>> result = new List<Tuple<IEntity<Guid>, string>>();
            foreach (var item in Data)
            {
                result.Add(new Tuple<IEntity<Guid>, string>(item.Item1, Message + " " + item.Item2));
            }
            return result;
        }

      
    }
}
