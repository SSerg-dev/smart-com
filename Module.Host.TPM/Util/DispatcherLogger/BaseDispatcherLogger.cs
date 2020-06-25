using Core.Data;
using Module.Host.TPM.Util.Interface;
using Module.Host.TPM.Util.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Host.TPM.Util.DispatcherLogger
{
    public class BaseDispatcherLogger : IDispatcherLogInfo<IList<string>>
    {
        public IList<ILogInfo> LogInfos { get; set; }

        public BaseDispatcherLogger()
        {
            LogInfos = new List<ILogInfo>();
        }

        public void Add<T>(IEntity<Guid> entity,string message, string column, string data) where T : class,ILogInfo,new()
        {
            lock (LogInfos)
            {
                var result = LogInfos.Where(e => e.Message.Equals(message)).FirstOrDefault();

                if (result == null)
                {


                    T log = new T();
                    log.Add(entity,message, column, data);
                    LogInfos.Add(log);

                }
                else
                {
                    result.AddData(entity, data);
                }
            }

        }
        public ConcurrentBag<Tuple<IEntity<Guid>, string>> GetErrorRecords()
        {
            ConcurrentBag<Tuple<IEntity<Guid>, string>> record = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
            foreach (var item in LogInfos.Where(e => e is IErrorLogger))
            {
                foreach (var item1 in item.BuildToRecord())
                {
                    record.Add(item1);
                }
               
            }
            return record;
        }
        public ConcurrentBag<Tuple<IEntity<Guid>, string>> GetWarningRecords()
        {
            ConcurrentBag<Tuple<IEntity<Guid>, string>> record = new ConcurrentBag<Tuple<IEntity<Guid>, string>>();
            foreach (var item in LogInfos.Where(e => e is IWarningLogger))
            {
                foreach (var item1 in item.BuildToRecord())
                {
                    record.Add(item1);
                }
            }
            return record;
        }
        
        public IList<string> GetError()
        {
            List<string> buffer = new List<string>();
            foreach (var item in LogInfos.Where(e => e is IErrorLogger))
            {
                buffer.Add(item.Build());
            }
            return buffer;
        }

        public IList<string> GetWarning()
        {
            List<string> buffer = new List<string>();
            foreach (var item in LogInfos.Where(e => e is IWarningLogger))
            {
                buffer.Add(item.Build());
            }
            return buffer;
        }

        
    }
}
