using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using System;

namespace AspNet.Identity.DocumentDb
{
    public static class IdentityDocumentDbBuilderExtensions
    {
        public static IdentityBuilder AddDocumentDbStores<TDocumentDb>(this IdentityBuilder builder, TDocumentDb docDb)
            where TDocumentDb : DocumentDbClient
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TDocumentDb)));
            builder.Services.AddInstance(docDb);
            return builder;
        }

        public static IdentityBuilder AddDocumentDbStores<TDocumentDb, TKey>(this IdentityBuilder builder, TDocumentDb docDb)
            where TDocumentDb : DocumentDbClient
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TDocumentDb), typeof(TKey)));
            builder.Services.AddInstance(docDb);
            return builder;
        }
    }
}