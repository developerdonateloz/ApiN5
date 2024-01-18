using ApiN5.Consumers;
using ApiN5.Producers;
using Azure.Core;
using Confluent.Kafka;
using Core.RequestParams;
using Core.Services;
using Core.Services.Impl;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiN5.Controllers
{
    [Route("permission")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger _logger;

        private readonly KafkaProducer _producer;
        private readonly KafkaConsumer _consumer;

        public PermissionController(IPermissionService permissionService,ILogger<PermissionController> logger,IConfiguration configuration)
        {
            _permissionService = permissionService;
            _logger = logger;

            var bootstrapServer = configuration["Kafka:BootstrapServers"];
            var groupId = configuration["Kafka:GroupId"];


        }
        [Route("request")]
        [HttpPost]
        public async Task<ActionResult> RequestPermission(PermissionRequestParams param)
        {
            _logger.LogInformation($"Service: RequestPermission");
            try
            {
                var response = await _permissionService.CreatePermission(param);
                _producer.ProduceAsync("Request Permission", "Status: OK").Wait();
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
            _logger.LogInformation($"Service: ModifyPermission");
            try
            {
                var response = await _permissionService.ModifyPermission(param);
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
            _logger.LogInformation($"Service: GetPermissions");
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
