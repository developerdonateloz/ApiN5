using Core.Dto;
using Core.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IPermissionTypeService
    {
        public Task<PermissionTypeDto> CreatePermissionType(PermissionTypeRequestParams param);
        public Task<PermissionTypeDto> ModifyPermissionType(PermissionTypeRequestParams param);
        public Task<List<PermissionTypeDto>> GetPermissionTypes(int? permissionTypeId);
    }
}
