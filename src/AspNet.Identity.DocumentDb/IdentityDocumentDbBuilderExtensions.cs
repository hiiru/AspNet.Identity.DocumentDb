using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using System;

namespace AspNet.Identity.DocumentDb
{
    public static class IdentityDocumentDbBuilderExtensions
    {
        public static IdentityBuilder AddDocumentDbStores(this IdentityBuilder builder, IIdentityDocumentDbClient docDb)
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType));
            builder.Services.AddInstance(docDb);
            return builder;
        }

        public static IdentityBuilder AddDocumentDbStores<TKey>(this IdentityBuilder builder, IIdentityDocumentDbClient docDb)
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TKey)));
            builder.Services.AddInstance(docDb);
            return builder;
        }
    }
}