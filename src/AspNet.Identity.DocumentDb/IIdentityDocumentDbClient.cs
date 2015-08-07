using AspNet.Identity.DocumentDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.Identity.DocumentDb
{
    public interface IIdentityDocumentDbClient
    {
        #region IdentityUser

        Task UserAdd<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : class, IEquatable<TKey>;

        Task UserDelete<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>;

        Task UserUpdate<TUser, TKey>(TUser user)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>;

        Task<IEnumerable<TUser>> UserSearch<TUser, TKey>(Expression<Func<TUser, bool>> predicate)
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>;

        IQueryable<TUser> UserQueryable<TUser, TKey>()
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>;

        #endregion IdentityUser

        #region IdentityRole
        
        Task RoleAdd<TRole>(TRole role) where TRole : IdentityRole;

        Task RoleDelete<TRole>(TRole role) where TRole : IdentityRole;

        Task RoleUpdate<TRole>(TRole role) where TRole : IdentityRole;

        Task<IEnumerable<TRole>> RoleSearch<TRole>(Expression<Func<TRole, bool>> predicate) where TRole : IdentityRole;

        IQueryable<TRole> RoleQueryable<TRole>() where TRole : IdentityRole;

        #endregion IdentityRole
    }
}
