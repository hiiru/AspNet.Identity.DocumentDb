using AspNet.Identity.DocumentDb.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.Identity.DocumentDb
{
    public class DocumentDbClient
    {
        private readonly DocumentClient _client;
        private readonly Database _db;
        private readonly DocumentCollection _collection;

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
        #region IdentityUser

        internal Task UserAdd<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task UserDelete<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task UserUpdate<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TUser>> UserSearch<TUser, TKey>(Expression<Func<TUser, bool>> predicate)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var query = (IDocumentQuery<TUser>)_client.CreateDocumentQuery<TUser>(_collection.DocumentsLink).Where(predicate);
            return await query.ExecuteNextAsync<TUser>();
        }

        internal IQueryable<TUser> UserQueryable<TUser, TKey>()
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            return _client.CreateDocumentQuery<TUser>(_collection.DocumentsLink);
        }
        #endregion
        #region IdentityRole

        internal Task RoleAdd<TRole, TKey>(TRole user)
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task RoleDelete<TRole, TKey>(TRole user)
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        internal Task RoleUpdate<TRole, TKey>(TRole user)
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TRole>> RoleSearch<TRole, TKey>(Expression<Func<TRole, bool>> predicate)
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var query = (IDocumentQuery<TRole>)_client.CreateDocumentQuery<TRole>(_collection.DocumentsLink).Where(predicate);
            return await query.ExecuteNextAsync<TRole>();
        }

        internal IQueryable<TRole> RoleQueryable<TRole, TKey>()
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            return _client.CreateDocumentQuery<TRole>(_collection.DocumentsLink);
        }
        #endregion
    }
}