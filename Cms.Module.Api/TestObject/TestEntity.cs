using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Data.Migration;
using OrchardCore.Entities;
using System.Collections.Generic;
using YesSql;
using YesSql.Indexes;
using YesSql.Sql;


namespace Cms.Module.Api.TestObject
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTestObject(this IServiceCollection services)
        {
            services.AddDataMigration<Migrations>();
            services.AddIndexProvider<TestObjectIndexProvider>();
            services.AddScoped<TestObjectManager>();
            services.Configure<StoreCollectionOptions>(o => o.Collections.Add(TestEntity.Collection));
            return services;
        }
    }
    public class TestObjectManager
    {
        private readonly ISession _session;

        public TestObjectManager(ISession session)
        {
            _session = session;
        }

        public async Task<List<TestEntity>> GetTestObjectByName(string name)
        {
            var items = await _session.Query<TestEntity, TestObjectIndex>(collection: TestEntity.Collection)
                .Where(item => item.Name != null && item.Name.StartsWith(name))
                .ListAsync();

            return items.ToList();
        }
        public async Task SaveTestObject(TestEntity testObject)
        {
            await _session.SaveAsync(testObject, TestEntity.Collection);
        }
    }
    public class TestEntity: Entity
    {
        public const string Collection = "TestObject";

        // this will be automatically set to the document id
        public long Id { get; set; }
        public string ObjectId { get; set; }  = Guid.NewGuid().ToString();

        public string Name { get; set; } = Path.GetRandomFileName();

        public string RandomString { get; set; } = $"{Path.GetRandomFileName()}-{DateTimeOffset.Now}";
    }
    public class TestObjectIndexProvider : IndexProvider<TestEntity>
    {
        public TestObjectIndexProvider() => CollectionName = TestEntity.Collection;

        public override void Describe(DescribeContext<TestEntity> context) =>
            context.For<TestObjectIndex>()
                .Map(test =>
                {
                    return new TestObjectIndex
                    {
                        Name = test.Name,
                        ObjectId = test.ObjectId,
                    };
                });
    }
    public class TestObjectIndex : MapIndex
    {
        public string? ObjectId { get; set; }

        public string? Name { get; set; }
    }
    public sealed class Migrations : DataMigration
    {
        public async Task<int> CreateAsync()
        {
            await SchemaBuilder.CreateMapIndexTableAsync<TestObjectIndex>(table => table
                .Column<string>(nameof(TestEntity.ObjectId), column => column.Unlimited())
                .Column<string>(nameof(TestEntity.Name), column => column.Unlimited()),
                collection: TestEntity.Collection);

            await SchemaBuilder.AlterIndexTableAsync<TestObjectIndex>(table => table
                .CreateIndex("IDX_TestObjectIndex_DocumentId",
                    "DocumentId",
                    "ObjectId",
                    "Name"
                    ),
                collection: TestEntity.Collection
            );

            return 1;
        }
    }
}
