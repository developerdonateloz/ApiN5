using Core.RequestParams;
using Core.Services.Impl;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Services
{
    public class PermissionServiceTest : IClassFixture<CoreTestFixture>
    {
        private readonly CoreTestFixture _fixture;
        private readonly ILogger<PermissionService> _logger;

        public PermissionServiceTest(CoreTestFixture fixture)
        {
            _fixture = fixture;
            _logger = new Mock<ILogger<PermissionService>>().Object;
        }

        [Fact(DisplayName = "Should create permission")]
        public async void CreatePermission()
        {
            var context = _fixture.CreateNewDbContext();

            var sut = new PermissionService(context, _logger);
            
            var requestPermissionParams = new PermissionRequestParams
            {
                EmployeeForename = "Dónaghy",
                EmployeeSurname = "Amachi",
                PermissionTypeId = 1,
                PermissionDate = default
            };
            var res = await sut.CreatePermission(requestPermissionParams);

            Assert.NotEqual(0, res.Id);
        }
    }
}
