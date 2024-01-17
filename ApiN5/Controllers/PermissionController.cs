using Core.RequestParams;
using Core.Services;
using Core.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace ApiN5.Controllers
{
    [Route("permission")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger _logger;
        public PermissionController(IPermissionService permissionService,ILogger<PermissionController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }
        [Route("request")]
        [HttpPost]
        public async Task<ActionResult> RequestPermission(PermissionRequestParams param)
        {
            try
            {
                var response = await _permissionService.CreatePermission(param);
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
        public async Task<ActionResult> ModifyPermission(PermissionRequestParams param)
        {
            try
            {
                var response = await _permissionService.CreatePermission(param);
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
        public async Task<ActionResult> GetPermissions(int? permissionId)
        {
            _logger.LogInformation("ORALE MANO");
            try
            {
                var response = await _permissionService.GetPermission(permissionId);
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
