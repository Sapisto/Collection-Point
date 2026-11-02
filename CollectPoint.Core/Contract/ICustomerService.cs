using CollectPoint.Core.DTO.Request;
using CollectPoint.Core.DTO.Response;
using CollectPoint.Domain.Common;
using CollectPoint.Domain.QueryParameters;

namespace CollectPoint.Core.Contract;

public interface ICustomerService
{
    Task<Response<List<CustomerResponse>>> GetCustomers(CustomerQueryParameters query, CancellationToken cancellationToken);
    Task<Response<CustomerResponse>> GetCustomerById(string id, CancellationToken cancellationToken);
    Task<Response<CustomerResponse>> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken);
}
