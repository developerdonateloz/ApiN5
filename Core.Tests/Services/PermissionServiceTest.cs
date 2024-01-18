using Core.DbModel.Entities;
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

            context.PermissionTypes.Add(new PermissionType
            {
                Id = 1,
                Description = "Type One"
            });
            await context.SaveChangesAsync();

            var requestPermissionParams = new PermissionRequestParams
            {
                EmployeeForename = "Dónaghy",
                EmployeeSurname = "Amachi",
                PermissionTypeId = 1,
            };
            var res = await sut.CreatePermission(requestPermissionParams);

            Assert.NotEqual(0, res.Id);
            Assert.Equal(1, context.Permissions.Count());
        }

        [Fact(DisplayName = "Should Modify Permission")]
        public async void ModifyPermission()
        {
            var context = _fixture.CreateNewDbContext();

            var sut = new PermissionService(context, _logger);

            context.PermissionTypes.Add(new PermissionType
            {
                Id = 1,
                Description = "First Type"
            });
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 2,
                Description = "Second Type"
            });

            context.Permissions.Add(new Permission
            {
                Id = 3,
                EmployeeForename = "Tony",
                EmployeeSurname = "Surname",
                PermissionTypeId = 1
            });

            await context.SaveChangesAsync();

            var requestPermissionParams = new PermissionRequestParams
            {
                Id = 3,
                EmployeeForename = "Steven",
                EmployeeSurname = "Rogers",
                PermissionTypeId = 2,
            };

            var res = await sut.ModifyPermission(requestPermissionParams);

            Assert.Equal(requestPermissionParams.Id, res.Id);
            Assert.Equal(requestPermissionParams.EmployeeForename, res.EmployeeForename);
            Assert.Equal(requestPermissionParams.EmployeeSurname, res.EmployeeSurname);
            Assert.Equal(requestPermissionParams.PermissionTypeId, res.PermissionTypeId);
        }

        [Fact(DisplayName = "Should Get Permissions")]
        public async void GetPermission()
        {
            var context = _fixture.CreateNewDbContext();

            var sut = new PermissionService(context, _logger);

            // ARRANGE
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 1,
                Description = "First Type"
            });
            context.PermissionTypes.Add(new PermissionType
            {
                Id = 2,
                Description = "Second Type"
            });

            context.Permissions.Add(new Permission
            {
                Id = 2,
                EmployeeForename = "Bruce",
                EmployeeSurname = "Banner",
                PermissionTypeId = 2
            });

            await context.SaveChangesAsync();

            // ACT
            var res2 = await sut.GetPermission(2);

            //ASSERT
            Assert.True(res2.Count == 1);
        }
    }
}
