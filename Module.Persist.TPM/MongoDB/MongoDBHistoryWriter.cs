using Core.Data;
using Core.Extensions;
using Core.History;
using Core.Security.Models;
using Microsoft.Ajax.Utilities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserInfo = Core.Security.Models.UserInfo;

namespace Module.Persist.TPM.MongoDB
{
    public class MongoDBHistoryWriter<TKey> : IHistoryWriter<TKey>
    {
        private readonly MongoHelper<TKey> MongoHelper;
        public MongoDBHistoryWriter(string uri, string dbName, double ttlSec, IHistoricalEntityFactory<TKey> historicalEntityFactory)
        {
            MongoHelper = new MongoHelper<TKey>(uri, dbName, ttlSec, historicalEntityFactory);
        }

        public void Write(IEnumerable<OperationDescriptor<TKey>> changes, UserInfo user, RoleInfo role)
        {
            foreach (var batch in changes.Where(x => x != null).Partition(1000))
            {
                foreach (var item in batch)
                {
                    MongoHelper.WriteChanges(item, user, role);
                }
            }
        }

        public async Task WriteAsync(IEnumerable<OperationDescriptor<TKey>> changes, UserInfo user, RoleInfo role)
        {
            foreach (var batch in changes.Where(x => x != null).Partition(1000))
            {
                foreach (var item in batch)
                {
                    await MongoHelper.WriteChangesAsync(item, user, role);
                }
            }
        }

        public void Write(IEnumerable<Tuple<IEntity<TKey>, IEntity<TKey>>> changes, UserInfo user, RoleInfo role, OperationType operation)
        {
            foreach (var batch in changes.Where(x => x != null).Partition(1000))
            {
                foreach (var item in batch)
                {
                    MongoHelper.WriteChangesFromImport(item, user, role, operation);
                }
            }
        }
    }
}