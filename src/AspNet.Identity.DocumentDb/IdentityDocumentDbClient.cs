using AspNet.Identity.DocumentDb.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.Identity.DocumentDb
{
    public class IdentityDocumentDbClient : IIdentityDocumentDbClient
    {
        private readonly DocumentClient _client;
        private Database _db;
        private DocumentCollection _collection;

        public IdentityDocumentDbClient(Uri endpoint, string key, string db = null, string collection = null, ConnectionPolicy policy = null)
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));
            if (key == null)
                throw new ArgumentNullException(nameof(endpoint));
            _client = new DocumentClient(endpoint, key, policy ?? new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
            EnsureDbSetup(db ?? "AspNetIdentity", collection ?? "AspNetIdentity");
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

        public async Task UserAdd<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : class, IEquatable<TKey>
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.DocSelfLink != null || user.DocId != null)
                throw new ArgumentException(nameof(user));
            if (user.Id == null || user.Id.Equals(default(TKey)))
            {
                if (typeof(TKey) == typeof(string))
                    user.Id = Guid.NewGuid().ToString() as TKey;
                else if (typeof(TKey) == typeof(Guid))
                    user.Id = Guid.NewGuid() as TKey;
                else
                    throw new ArgumentException("No UserId is set!", nameof(user));
            }
            user.DocId = string.Concat(UserPrefix, user.Id);
            var doc = await _client.CreateDocumentAsync(_collection.DocumentsLink, user);
            user.DocSelfLink = doc.Resource.SelfLink;
            user.SecurityStamp = doc.Resource.Timestamp.Ticks.ToString();
            return;
        }

        public async Task UserDelete<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.DocSelfLink == null || user.DocId == null)
                throw new ArgumentException(nameof(user));
            await _client.DeleteDocumentAsync(user.DocSelfLink);
        }

        public async Task UserUpdate<TUser, TKey>(TUser user)
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

        public IQueryable<TUser> UserQueryable<TUser, TKey>()
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
        {
            return _client.CreateDocumentQuery<TUser>(_collection.DocumentsLink);
        }

        #endregion IdentityUser

        #region IdentityRole

        public async Task RoleAdd<TRole>(TRole role) where TRole : IdentityRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.DocSelfLink != null || role.DocId != null)
                throw new ArgumentException(nameof(role));
            role.DocId = string.Concat(RolePrefix, role.Name);
            await _client.CreateDocumentAsync(_collection.DocumentsLink, role);
        }

        public async Task RoleDelete<TRole>(TRole role) where TRole : IdentityRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            if (role.DocSelfLink == null || role.DocId == null)
                throw new ArgumentException(nameof(role));
            await _client.DeleteDocumentAsync(role.DocSelfLink);
        }

        public async Task RoleUpdate<TRole>(TRole role) where TRole : IdentityRole
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

        public IQueryable<TRole> RoleQueryable<TRole>() where TRole : IdentityRole
        {
            return _client.CreateDocumentQuery<TRole>(_collection.DocumentsLink);
        }

        #endregion IdentityRole
    }
}