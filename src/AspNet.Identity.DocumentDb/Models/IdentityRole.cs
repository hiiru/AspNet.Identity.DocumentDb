using System;

namespace AspNet.Identity.DocumentDb.Models
{/// <summary>
 ///     Represents a Role entity
 /// </summary>
    public class IdentityRole : IdentityRole<string> { }

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
    }
}