using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using System;

namespace AspNet.Identity.DocumentDb
{
    public static class IdentityDocumentDbBuilderExtensions
    {
        public static IdentityBuilder AddDocumentDbStores<TDocumentDb>(this IdentityBuilder builder)
            where TDocumentDb : DocumentDbClient
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TDocumentDb)));
            return builder;
        }

        public static IdentityBuilder AddDocumentDbStores<TDocumentDb, TKey>(this IdentityBuilder builder)
            where TDocumentDb : DocumentDbClient
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TDocumentDb), typeof(TKey)));
            return builder;
        }
    }
}