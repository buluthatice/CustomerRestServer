using CustomerRestServer.Dto;

namespace CustomerRestServer.DataAccess
{
    public interface ICustomerAccess
    {
        void InsertCustomer(List<CustomerDto> customers);
        List<CustomerDto> GetCustomers();
    }
}
