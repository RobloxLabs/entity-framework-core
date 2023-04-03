using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Unit.Tests.Databases
{
    public class RobloxTest : GlobalDatabase<RobloxTest>
    {
        private const string _DbPath = "Databases/RobloxTest.db";

        public RobloxTest() : base(
            connectionString: $"Data Source={_DbPath}",
            DatabaseType.Sqlite
        )
        { }
    }
}
