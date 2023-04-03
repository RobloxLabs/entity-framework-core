using Roblox.EntityFrameworkCore.Entities;
using Roblox.EntityFrameworkCore.Unit.Tests.Databases;

namespace Roblox.EntityFrameworkCore.Unit.Tests.Entities
{
    /// <summary>
    /// Represents a type of creator on the ROBLOX platform.
    /// </summary>
    public class CreatorType : EnumeratorEntity<CreatorType, byte, RobloxTest>
    {
        /// <summary>
        /// Represents a user creator
        /// </summary>
        public static readonly CreatorType User = GetOrCreate(nameof(User));

        /// <summary>
        /// Represents a group creator
        /// </summary>
        public static readonly CreatorType Group = GetOrCreate(nameof(Group));
    }
}
