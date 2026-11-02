using Microsoft.AspNetCore.Mvc;
using CollectPoint.Core.Contract;
using CollectPoint.Core.DTO.Request;
using CollectPoint.Core.DTO.Response;
using CollectPoint.Domain.Common;
using CollectPoint.Domain.QueryParameters;

namespace CollectPoint.Api.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("customers")]
    public async Task<ActionResult<Response<List<CustomerResponse>>>> GetCustomers([FromQuery] CustomerQueryParameters query)
        => Ok(await _customerService.GetCustomers(query, HttpContext.RequestAborted));

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<CustomerResponse>>> GetCustomerById(string id)
        => Ok(await _customerService.GetCustomerById(id, HttpContext.RequestAborted));

    [HttpPost]
    public async Task<ActionResult<Response<CustomerResponse>>> CreateCustomer([FromBody] CreateCustomerRequest request)
        => Ok(await _customerService.CreateCustomer(request, HttpContext.RequestAborted));
}
