using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Identity.DocumentDb.Models;
using System.Linq.Expressions;
using Microsoft.Azure.Documents.Client;
using Microsoft.Framework.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace AspNet.Identity.DocumentDb
{
    public class DocumentDbClient
    {
        private readonly DocumentClient _client;
        private Database _db;
        private DocumentCollection _collection;

        public DocumentDbClient(IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var url = config["DocumentDb:EndpointUrl"];
            var key = config["DocumentDb:AuthorizationKey"];
            var db = config["DocumentDb:Database"];
            var collection = config["DocumentDb:Collection"];
            if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(db) || string.IsNullOrWhiteSpace(collection))
                throw new ArgumentException(nameof(config));
            _client = new DocumentClient(new Uri(url), key, new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            //EnsureDbSetup(db, collection);
            //EnsureSpSetup();
        }

        internal Task Add<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task Delete<TUser, TKey>(IdentityUser<TKey> user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task Update<TUser, TKey>(IdentityUser<TKey> user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TUser>> Search<TUser, TKey>(Expression<Func<TUser, bool>> predicate)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var query = (IDocumentQuery<TUser>)_client.CreateDocumentQuery<TUser>(_collection.DocumentsLink).Where(predicate); 
            return await query.ExecuteNextAsync<TUser>();
        }
    }
}
