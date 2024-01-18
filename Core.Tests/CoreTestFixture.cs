using Core.DbModel.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;

namespace Core.Tests
{
    public class CoreTestFixture
    {
        private readonly Random _random;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public CoreTestFixture()
        {
            _random=new Random(Environment.TickCount);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            IConfiguration configuration = new ConfigurationBuilder()
                .Build();

            _context = new DataContext(options, configuration);
            _configuration = configuration;
        }
        public DataContext CreateNewDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            //var inMemorySettings = new Dictionary<string, string> {
            //    {"ENTORNO", "test"},
            //};

            IConfiguration configuration = new ConfigurationBuilder()
                //.AddInMemoryCollection(inMemorySettings)
                .Build();

            return new DataContext(options, configuration);
        }
    }
}
