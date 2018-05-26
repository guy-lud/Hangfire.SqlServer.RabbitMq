using System;
using System.Linq;
using Hangfire.PostgreSql;
using Xunit;

namespace Hangfire.Postgres.RabbitMq.Tests
{
    public class RabbitMqSqlServerStorageExtensionsFacts : IDisposable
    {
        private readonly PostgreSqlStorage _storage;

        public RabbitMqSqlServerStorageExtensionsFacts()
        {
            _storage = new PostgreSqlStorage(
                @"Server=.\sqlexpress;Database=TheDatabase;Trusted_Connection=True;",
                new PostgreSqlStorageOptions(){ PrepareSchemaIfNecessary = false});
        }

        [Fact]
        public void UseRabbitMq_ThrowsAnException_WhenStorageIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => RabbitMqPostgreSqlStorageExtensions.UseRabbitMq(null, conf => conf.HostName = "localhost"));
            
            Assert.Equal("storage", exception.ParamName);
        }

        [Fact]
        public void UseRabbitMq_AddsRabbitMqJobQueueProvider()
        {
            _storage.UseRabbitMq(conf => conf.HostName = "localhost", "default");

            var providerTypes = _storage.QueueProviders.Select(x => x.GetType());
            Assert.Contains(typeof(RabbitMqJobQueueProvider), providerTypes);
        }

        public void Dispose()
        {
            foreach (var provider in _storage.QueueProviders)
            {
                (provider as IDisposable)?.Dispose();
            }
        }
    }
}
