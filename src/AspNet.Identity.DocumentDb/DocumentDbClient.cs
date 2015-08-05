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
            EnsureDbSetup(db, collection);
        }

        public DocumentDbClient(Uri endpoint, string key, string db = "AspNetIdentity", string collection = "AspNetIdentity")
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));
            if (key == null)
                throw new ArgumentNullException(nameof(endpoint));
            _client = new DocumentClient(endpoint, key, new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            EnsureDbSetup(db, collection);
        }

        private async void EnsureDbSetup(string dbId, string collectionId)
        {
            _db = _client.CreateDatabaseQuery().Where(x => x.Id == dbId).AsEnumerable().FirstOrDefault();
            if (_db == null)
                _db = await _client.CreateDatabaseAsync(new Database { Id = dbId });
            _collection = _client.CreateDocumentCollectionQuery(_db.CollectionsLink).Where(x => x.Id == collectionId).AsEnumerable().FirstOrDefault();
            if (_collection == null)
                _collection = await _client.CreateDocumentCollectionAsync(_db.CollectionsLink, new DocumentCollection { Id = collectionId });
        }

        public string UserPrefix { get; set; } = "user_";
        public string RolePrefix { get; set; } = "role_";

        #region IdentityUser
        
        private string GetUserKey<TKey>(TKey userId)
        {
            if (userId.Equals(default(TKey)))
                return null;
            return string.Concat(UserPrefix, userId);
        }

        internal async Task UserAdd<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.DocSelfLink != null || user.DocId != null)
                throw new ArgumentException(nameof(user));
            user.DocId = GetUserKey(user.DocId);
            await _client.CreateDocumentAsync(_collection.DocumentsLink, user);
        }

        internal async Task UserDelete<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.DocSelfLink == null || user.DocId == null)
                throw new ArgumentException(nameof(user));
            await _client.DeleteDocumentAsync(user.DocSelfLink);
        }

        internal async Task UserUpdate<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.DocSelfLink == null || user.DocId == null)
                throw new ArgumentException(nameof(user));
            await _client.ReplaceDocumentAsync(user.DocSelfLink, user);
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

        #endregion IdentityUser

        #region IdentityRole

        internal async Task RoleAdd<TRole>(TRole role) where TRole : IdentityRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.DocSelfLink != null || role.DocId != null)
                throw new ArgumentException(nameof(role));
            role.DocId = string.Concat(RolePrefix, role.Name);
            await _client.CreateDocumentAsync(_collection.DocumentsLink, role);
        }

        internal async Task RoleDelete<TRole>(TRole role) where TRole : IdentityRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.DocSelfLink == null || role.DocId == null)
                throw new ArgumentException(nameof(role));
            await _client.DeleteDocumentAsync(role.DocSelfLink);
        }

        internal async Task RoleUpdate<TRole>(TRole role) where TRole : IdentityRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.DocSelfLink == null || role.DocId == null)
                throw new ArgumentException(nameof(role));
            await _client.ReplaceDocumentAsync(role.DocSelfLink, role);
        }

        public async Task<IEnumerable<TRole>> RoleSearch<TRole>(Expression<Func<TRole, bool>> predicate) where TRole : IdentityRole
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var query = (IDocumentQuery<TRole>)_client.CreateDocumentQuery<TRole>(_collection.DocumentsLink).Where(predicate);
            return await query.ExecuteNextAsync<TRole>();
        }

        internal IQueryable<TRole> RoleQueryable<TRole>() where TRole : IdentityRole
        {
            return _client.CreateDocumentQuery<TRole>(_collection.DocumentsLink);
        }

        #endregion IdentityRole
    }
}