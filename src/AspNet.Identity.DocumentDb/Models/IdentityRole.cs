﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.DocumentDb.Models
{
    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    public class IdentityRole
    {
        public IdentityRole()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="roleName"></param>
        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        /// <summary>
        ///     Role Claims
        /// </summary>
        public virtual ICollection<Claim> Claims { get; } = new List<Claim>();

        /// <summary>
        ///     Role name
        /// </summary>
        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// Returns a friendly name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
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