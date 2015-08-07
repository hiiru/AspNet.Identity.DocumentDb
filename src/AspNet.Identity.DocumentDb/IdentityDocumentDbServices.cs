using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;
using System;

namespace AspNet.Identity.DocumentDb
{
    public class IdentityDocumentDbServices
    {
        public static IServiceCollection GetDefaultServices(Type userType, Type roleType, Type keyType = null)
        {
            Type userStoreType;
            Type roleStoreType;
            if (keyType != null)
            {
                userStoreType = typeof(UserStore<,,>).MakeGenericType(userType, roleType, keyType);
            }
            else
            {
                userStoreType = typeof(UserStore<,>).MakeGenericType(userType, roleType);
            }
            roleStoreType = typeof(RoleStore<>).MakeGenericType(roleType);

            var services = new ServiceCollection();
            services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(userType),
                userStoreType);
            services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(roleType),
                roleStoreType);
            return services;
        }
    }
}