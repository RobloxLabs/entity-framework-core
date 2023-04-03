using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Unit.Tests.Databases
{
    public class RobloxMySqlTest : GlobalDatabase<RobloxMySqlTest>
    {
        public RobloxMySqlTest() : base(
            connectionString: $"server=localhost;database=RobloxMaster;user=root;password=",
            DatabaseType.MySql
        )
        { }
    }
}
