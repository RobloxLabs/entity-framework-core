using Roblox.EntityFrameworkCore.Entities;
using Roblox.EntityFrameworkCore.Unit.Tests.Entities;

namespace Roblox.EntityFrameworkCore.Unit.Tests
{
    public class CreatorTypeTests
    {
        private void ValidateEntity<TEntity>(TEntity? expected, TEntity? actual)
            where TEntity : IEnumeratorEntity<byte>
        {
            if (expected == null)
            {
                Assert.Null(actual);
            }
            else
            {
                Assert.NotNull(actual);

                Assert.NotEqual(default, actual.ID);
                Assert.True(actual.ID > 0, "Entity ID is somehow <= 0!");

                if (!expected.ID.Equals(default))
                    Assert.Equal(expected.ID, actual.ID);

                if (!string.IsNullOrEmpty(expected.Value))
                    Assert.Equal(expected.Value, actual.Value);
            }
        }

        [Fact]
        public void ValidateCreatorTypes()
        {
            ValidateEntity(
                CreatorType.User,
                new CreatorType
                {
                    ID = 1,
                    Value = "User"
                }
            );
            ValidateEntity(
                CreatorType.Group,
                new CreatorType
                {
                    ID = 2,
                    Value = "Group"
                }
            );
        }

        [Theory]
        [InlineData("User2")]
        [InlineData("Group2")]
        public void GetCreatorTypeByID(string expectedValue)
        {
            var creatorTypeTemp = CreatorType.Create(expectedValue);
            var creatorType = CreatorType.Get(creatorTypeTemp.ID);
            creatorTypeTemp.Delete();

            ValidateEntity
            (
                actual: creatorType,
                expected: new CreatorType { ID = creatorTypeTemp.ID, Value = expectedValue }
            );
        }

        [Theory]
        [InlineData("User3")]
        [InlineData("Group3")]
        public void GetOrCreateCreatorTypeByValue(string value)
        {
            var creatorType = CreatorType.GetOrCreate(value);
            creatorType.Delete();

            ValidateEntity
            (
                actual: creatorType,
                expected: new CreatorType { Value = value }
            );
        }

        public ICollection<CreatorType> GetCreatorTypes()
        {
            var result = CreatorType.GetAll();

            Assert.NotEmpty(result);
            return result;
        }

        [Theory]
        [InlineData("User4")]
        [InlineData("Group4")]
        public void CreatorTypesContains(string expectedValue)
        {
            var creatorType = CreatorType.Create(expectedValue);
            var creatorTypes = GetCreatorTypes();
            creatorType.Delete();

            Assert.Contains(creatorTypes, (entity) => entity.Value == expectedValue);
        }
    }
}