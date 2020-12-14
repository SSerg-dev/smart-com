using Core.History;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.MongoDB
{
    public class DocumentStoreHolder
    {
        private static MongoClient Mongo = null;
        private static IMongoDatabase DB = null;
        private static object Collection = null; 

        private static bool Connected = false;
        private static double TTLSec = 63113904;

        public static MongoClient GetConnection(string uri, double ttlSec)
        {
            TTLSec = ttlSec;

            if (!Connected)
            {
                CreateConnection(uri);
            }

            return Mongo;
        }

        public static IMongoDatabase GetDatabase(string dbName)
        {
            if (Mongo == null)
                return null;

            if (DB == null)
                DB = Mongo.GetDatabase(dbName);

            return DB;
        }

        public static IMongoCollection<T> GetCollection<T>() where T : BaseHistoricalEntity<Guid>
        {
            if (Mongo == null || DB == null)
                return null;

            string collectionName = typeof(T).Name.ToLower();

            if (!CollectionExists(collectionName))
            {
                DB.CreateCollection(collectionName);
                var collection = DB.GetCollection<T>(collectionName);
                CreateIndex(collection);
                Collection = collection;
            }
            else
                SetCurrentCollection<T>(collectionName);

            return (IMongoCollection<T>)Collection;
        }

        public static IMongoCollection<object> GetCollection(string collectionName)
        {
            if (Mongo == null || DB == null)
                return null;

            if (!CollectionExists(collectionName))
            {
                DB.CreateCollection(collectionName);
                var collection = DB.GetCollection<object>(collectionName);
                CreateIndex(collection);
                Collection = collection;
            }
            else
                SetCurrentCollection(collectionName);

            return (IMongoCollection<object>)Collection;
        }

        private static bool CollectionExists(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var collectionCursor = DB.ListCollections(new ListCollectionsOptions { Filter = filter });
            return collectionCursor.Any();
        }

        private static void CreateConnection(string uri)
        {
            try
            {
                Mongo = new MongoClient(uri);
                Connected = true;
            }
            catch (Exception e)
            {
                Connected = false;
                Console.Write(e);
            }
        }

        private static void SetCurrentCollection(string collectionName)
        {
            if (Collection == null || !(Collection is IMongoCollection<object>))
            {
                Collection = DB.GetCollection<object>(collectionName);
                return;
            }

            var collection = (IMongoCollection<object>)Collection;
            if (collection.CollectionNamespace.CollectionName.Equals(collectionName))
                return;

            Collection = DB.GetCollection<object>(collectionName);
        }

        private static void SetCurrentCollection<T>(string collectionName) where T : BaseHistoricalEntity<Guid>
        {
            if (Collection == null || !(Collection is IMongoCollection<T>))
            {
                Collection = DB.GetCollection<T>(collectionName);
                return;
            }

            var collection = (IMongoCollection<T>)Collection;
            if (collection.CollectionNamespace.CollectionName.Equals(collectionName))
                return;

            Collection = DB.GetCollection<T>(collectionName);
        }

        private static void CreateIndex<T>(IMongoCollection<T> collection) where T : class
        {
            CreateIndexModel<T> ttlIndex = new CreateIndexModel<T>(
                Builders<T>.IndexKeys.Ascending("_ts"),
                new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(TTLSec) }
            );
            CreateIndexModel<T> ascIndex = new CreateIndexModel<T>(
                Builders<T>.IndexKeys.Combine(
                    Builders<T>.IndexKeys.Ascending("_ObjectId"),
                    Builders<T>.IndexKeys.Ascending("_EditDate")
            ));

            collection.Indexes.CreateMany(new CreateIndexModel<T>[] { ttlIndex, ascIndex });

        }
    }
}
