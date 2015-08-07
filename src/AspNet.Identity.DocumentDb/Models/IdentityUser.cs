using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.DocumentDb.Models
{
    public class IdentityUser : IdentityUser<string> { }

    public class IdentityUser<TKey>  where TKey : IEquatable<TKey> 
    {
        public IdentityUser()
        {
        }

        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }

        public virtual TKey Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public virtual string Email { get; set; }

        public virtual string NormalizedEmail { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// SecurityStamp, use the DocumentDb timestamp to be sure it's updated when any data is changed
        /// </summary>
        [JsonProperty(PropertyName = "_ts")]
        public string SecurityStamp { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        ///     User Roles
        /// </summary>
        public virtual ICollection<string> Roles { get; } = new List<string>();

        /// <summary>
        ///     User Claims
        /// </summary>
        public virtual ICollection<Claim> Claims { get; } = new List<Claim>();

        /// <summary>
        ///     User Logins
        /// </summary>
        public virtual ICollection<UserLoginInfo> Logins { get; } = new List<UserLoginInfo>();

        /// <summary>
        /// Returns a friendly name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserName;
        }

        /// <summary>
        /// DocumentDb unique id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string DocId { get; set; }

        /// <summary>
        /// cached DocumentDb SelfLink for faster access
        /// </summary>
        [JsonProperty(PropertyName = "_self")]
        public string DocSelfLink { get; set; }
    }
}