using Core.RequestParams;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiN5.Controllers
{
    [Route("permissiontype")]
    public class PermissionTypeController : Controller
    {
        private readonly IPermissionTypeService _permissionTypeService;
        
        public PermissionTypeController(IPermissionTypeService permissionTypeService)
        {
            _permissionTypeService = permissionTypeService;
        }

        [Route("request")]
        [HttpPost]
        public async Task<ActionResult> RequestPermission(PermissionTypeRequestParams param)
        {
            try
            {
                var response = await _permissionTypeService.CreatePermissionType(param);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, $"Internal error:{ex.Message}");
            }
        }
        
        [Route("modify")]
        [HttpPut]
        public async Task<ActionResult> ModifyPermissionType(PermissionTypeRequestParams param)
        {
            try
            {
                var response = await _permissionTypeService.CreatePermissionType(param);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, $"Internal error:{ex.Message}");
            }
        }
        
        [Route("get")]
        [HttpGet]
        public async Task<ActionResult> GetPermissionTypes(int? permissionTypeId)
        {
            try
            {
                var response = await _permissionTypeService.GetPermissionTypes(permissionTypeId);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, $"Internal error:{ex.Message}");
            }
        }
    }
}
