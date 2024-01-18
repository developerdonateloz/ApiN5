using Core.DbModel.Context;
using Core.DbModel.Entities;
using Core.Dto;
using Core.RequestParams;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Services.Impl
{
    public class PermissionService : IPermissionService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;
        public PermissionService(DataContext dataContext, ILogger<PermissionService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<PermissionDto> CreatePermission(PermissionRequestParams param)
        {
            var permissionTypeExist = await _dataContext.PermissionTypes
                .AnyAsync(q => q.Id == param.PermissionTypeId);

            if (!permissionTypeExist)
            {
                _logger.LogError($"PermissionType with Id: {param.PermissionTypeId} doesn't exist.");

                throw new BadHttpRequestException($"PermissionType with Id: {param.PermissionTypeId} doesn't exist.");
            }

            if (string.IsNullOrWhiteSpace(param.EmployeeForename))
            {
                _logger.LogError($"EmployeeForename can't be empty.");
                throw new BadHttpRequestException($"EmployeeForename can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(param.EmployeeSurname))
            {
                _logger.LogError($"EmployeeSurname can't be empty.");
                throw new BadHttpRequestException($"EmployeeSurname can't be empty.");
            }

            var permissionExist = await _dataContext.Permissions
                .AnyAsync(q => q.EmployeeForename == param.EmployeeForename
                && q.EmployeeSurname == param.EmployeeSurname
                && q.PermissionTypeId == param.PermissionTypeId);

            if (permissionExist)
            {
                _logger.LogError($"Permission is duplicated.");
                throw new BadHttpRequestException($"Permission is duplicated.");
            }

            var permission = new Permission
            {
                EmployeeForename = param.EmployeeForename,
                EmployeeSurname = param.EmployeeSurname,
                PermissionTypeId = param.PermissionTypeId,
                PermissionDate = DateTime.UtcNow
            };

            _dataContext.Add(permission);
            await _dataContext.SaveChangesAsync();

            return new PermissionDto
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate
            };
        }

        public async Task<PermissionDto> ModifyPermission(PermissionRequestParams param)
        {
            var permission = await _dataContext.Permissions
                .FirstOrDefaultAsync(q => q.Id == param.Id);

            if (permission == null)
            {
                _logger.LogError($"Permission with Id: {param.Id} doesn't exist.");
                throw new BadHttpRequestException($"Permission with Id: {param.Id} doesn't exist.");
            }

            permission.EmployeeForename = param.EmployeeForename;
            permission.EmployeeSurname = param.EmployeeSurname;
            permission.PermissionTypeId = param.PermissionTypeId;
            permission.PermissionDate = DateTime.UtcNow;

            _dataContext.Entry(permission).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();

            return new PermissionDto
            {
                Id = permission.Id,
                EmployeeForename = permission.EmployeeForename,
                EmployeeSurname = permission.EmployeeSurname,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate
            };
        }

        public async Task<List<PermissionDto>> GetPermission(int? permissionId)
        {
            _logger.LogInformation("Service GetPermissions");

            var permission = _dataContext.Permissions
                .Join(_dataContext.PermissionTypes, x => x.PermissionTypeId, y => y.Id,
                    (x, y) => new
                    {
                        permissions = x,
                        permissiontype = y
                    })
                .Select(s => new PermissionDto
                {
                    Id = s.permissions.Id,
                    EmployeeForename = s.permissions.EmployeeForename,
                    EmployeeSurname = s.permissions.EmployeeSurname,
                    PermissionTypeId = s.permissions.PermissionTypeId,
                    PermissionTypeDescription = s.permissiontype.Description,
                    PermissionDate = s.permissions.PermissionDate
                });

            if (permissionId != null)
            {
                permission = permission.Where(q => q.Id == permissionId);
            }

            var listPermissions = await permission.ToListAsync();

            if (listPermissions.Count == 0)
            {
                if (permissionId != null)
                {
                    _logger.LogError($"Permissions with Id: {permissionId} doesn't exist.");
                    throw new BadHttpRequestException($"Permission with Id: {permissionId} doesn't exist.");
                }
                else
                {
                    _logger.LogError($"No permissions found.");
                    throw new BadHttpRequestException($"No permissions found.");
                }
            }

            return listPermissions;
        }
    }
}
