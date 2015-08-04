using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using System;

namespace AspNet.Identity.DocumentDb
{
    public static class IdentityDocumentDbBuilderExtensions
    {
        public static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DocumentDbClient
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext)));
            return builder;
        }

        public static IdentityBuilder AddEntityFrameworkStores<TContext, TKey>(this IdentityBuilder builder)
            where TContext : DocumentDbClient
            where TKey : IEquatable<TKey>
        {
            builder.Services.TryAdd(IdentityDocumentDbServices.GetDefaultServices(builder.UserType, builder.RoleType, typeof(TContext), typeof(TKey)));
            return builder;
        }
    }
}