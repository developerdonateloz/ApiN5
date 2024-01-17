using Core.Dto;
using Core.RequestParams;

namespace Core.Services
{
    public interface IPermissionService
    {
        public Task<PermissionDto> CreatePermission(PermissionRequestParams param);
        public Task<PermissionDto> ModifyPermission(PermissionRequestParams param);
        public Task<List<PermissionDto>> GetPermission(int? permissionId);
    }
}
