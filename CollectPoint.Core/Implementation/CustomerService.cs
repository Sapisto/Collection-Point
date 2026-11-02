using System.Net;
using Microsoft.Extensions.Logging;
using CollectPoint.Core.Contract;
using CollectPoint.Core.Contract.Repository;
using CollectPoint.Core.DTO.Request;
using CollectPoint.Core.DTO.Response;
using CollectPoint.Core.Exceptions;
using CollectPoint.Domain.Common;
using CollectPoint.Domain.Entities;
using CollectPoint.Domain.QueryParameters;

namespace CollectPoint.Core.Implementation;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    // Constructor injection, exactly like Nest
    public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<Response<List<CustomerResponse>>> GetCustomers(CustomerQueryParameters query, CancellationToken cancellationToken)
    {
        // Guard against pageSize=0 or a huge page
        query.PageNumber = Math.Max(query.PageNumber, 1);
        query.PageSize = Math.Clamp(query.PageSize, 1, 100);

        var (items, total) = await _customerRepository.GetCustomersAsync(query, cancellationToken);

        return new Response<List<CustomerResponse>>(items.Select(CustomerResponse.From).ToList(), "Customers retrieved successfully")
        {
            PageMeta = new PageMeta
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = total,
                TotalPages = (int)Math.Ceiling(total / (double)query.PageSize)
            }
        };
    }

    public async Task<Response<CustomerResponse>> GetCustomerById(string id, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ApiException($"Customer {id} was not found.", 404, HttpStatusCode.NotFound);

        return new Response<CustomerResponse>(CustomerResponse.From(customer));
    }

    public async Task<Response<CustomerResponse>> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.AccountNumberExistsAsync(request.AccountNumber, cancellationToken))
        {
            // FIRSPortal style: a business failure goes in the body, not the HTTP status
            return new Response<CustomerResponse>($"AccountNumber {request.AccountNumber} already exists.", 400);
        }

        var customer = new Customer
        {
            CustomerName = request.CustomerName.Trim(),
            AccountNumber = request.AccountNumber,
            Currency = request.Currency
        };

        await _customerRepository.AddAsync(customer, cancellationToken);

        _logger.LogInformation("Customer {CustomerId} created.", customer.Id);

        return new Response<CustomerResponse>(CustomerResponse.From(customer), "Customer created successfully");
    }
}
