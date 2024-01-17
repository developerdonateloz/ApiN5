using Core.DbModel.Context;
using Core.DbModel.Entities;
using Core.Dto;
using Core.RequestParams;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Services.Impl
{
    public class PermissionTypeService : IPermissionTypeService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;

        public PermissionTypeService(DataContext dataContext, ILogger<PermissionTypeService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<PermissionTypeDto> CreatePermissionType(PermissionTypeRequestParams param)
        {
            if (string.IsNullOrWhiteSpace(param.Description))
            {
                _logger.LogError($"EmployeeForename can't be empty.");
                throw new BadHttpRequestException($"EmployeeForename can't be empty.");
            }

            var permissionTypeExist = await _dataContext.PermissionTypes
                .AnyAsync(q => q.Description == param.Description);

            if (permissionTypeExist)
            {
                _logger.LogError($"PermissionType with Description: {param.Description} can't be duplicated.");

                throw new BadHttpRequestException($"PermissionType with Description: {param.Description} can't be duplicated.");
            }


            var permissionType = new PermissionType
            {
                Description = param.Description,
            };

            _dataContext.Add(permissionType);
            await _dataContext.SaveChangesAsync();

            return new PermissionTypeDto
            {
                Id = permissionType.Id,
                Description = param.Description
            };
        }

        public async Task<PermissionTypeDto> ModifyPermissionType(PermissionTypeRequestParams param)
        {
            var permissionType = await _dataContext.PermissionTypes
                .FirstOrDefaultAsync(q => q.Id == param.Id);

            if (permissionType == null)
            {
                _logger.LogError($"PermissionType with Id: {param.Id} doesn't exist.");
                throw new BadHttpRequestException($"PermissionType with Id: {param.Id} doesn't exist.");
            }

            permissionType.Description = param.Description;

            _dataContext.Entry(permissionType).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            return new PermissionTypeDto
            {
                Id = permissionType.Id,
                Description = param.Description
            };
        }

        public async Task<List<PermissionTypeDto>> GetPermissionTypes(int? permissionTypeId)
        {
            _logger.LogInformation("GetPermissionTypes Service");
            var permissionTypes = _dataContext.PermissionTypes.AsQueryable();

            if (permissionTypeId != null)
            {
                permissionTypes = permissionTypes.Where(q => q.Id == permissionTypeId);
            }

            var listPermissionTypes = await permissionTypes.Select(s => new PermissionTypeDto
            {
                Id = s.Id,
                Description = s.Description
            }).ToListAsync();

            if (listPermissionTypes.Count == 0)
            {
                if (permissionTypeId != null)
                {
                    _logger.LogError($"Permissions with Id: {permissionTypeId} doesn't exist.");
                    throw new BadHttpRequestException($"Permission with Id: {permissionTypeId} doesn't exist.");
                }
                else
                {
                    _logger.LogError($"No permissions found.");
                    throw new BadHttpRequestException($"No permissions found.");
                }
            }

            return listPermissionTypes;
        }
    }
}
