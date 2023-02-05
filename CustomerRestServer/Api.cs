using CustomerRestServer.DataAccess;
using CustomerRestServer.Dto;

namespace CustomerRestServer
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/Customers", GetCustomers);
            app.MapPost("/Customers", InsertCustomers);
        }

        private static IResult GetCustomers(ICustomerAccess data)
        {
            try
            {
                var results = data.GetCustomers();
                if (results == null) return Results.NotFound();
                else return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static IResult InsertCustomers(List<CustomerDto> customers, ICustomerAccess data)
        {
            try
            {
                data.InsertCustomer(customers);
                return Results.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}