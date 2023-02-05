using CustomerRestServer.Dto;
using System.IO;
using System.Text.Json;

namespace CustomerRestServer.DataAccess
{
    public class CustomerAccess : ICustomerAccess
    {
        private readonly List<CustomerDto> _insertedCustomer;
        private const string _fileName = "Customers.json";
        
        /// <summary>
        /// Customer access class constructor
        /// Should read customer list from json file for persist array
        /// </summary>
        public CustomerAccess()
        {
            if (File.Exists(_fileName))
            {
                string jsonFromFile = File.ReadAllText(_fileName);
                _insertedCustomer = string.IsNullOrEmpty(jsonFromFile) ? new List<CustomerDto>() : JsonSerializer.Deserialize<List<CustomerDto>>(jsonFromFile);
            }
            else { _insertedCustomer = new List<CustomerDto>(); }
        }

        /// <summary>
        /// Insert new customer array to exist array
        /// contains validation, finding right index and saving
        /// </summary>
        /// <param name="customers"></param>
        public void InsertCustomer(List<CustomerDto> customers)
        {
            foreach (var customer in customers)
            {
                ValidateCustomer(customer);
                InsertCustomer(customer);
            }
            SaveCustomer();
        }
        /// <summary>
        /// Get all the customers from already saved array
        /// </summary>
        /// <returns></returns>
        public List<CustomerDto> GetCustomers()
        {
            return _insertedCustomer;
        }
        /// <summary>
        /// Validate a customer, and return an exception with an inform
        /// </summary>
        /// <param name="customer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        void ValidateCustomer(CustomerDto customer)
        {
            if (customer == null)
                throw new ArgumentNullException("Customer cannot be null, please check!");
            else if (customer.Id == 0)
                throw new ArgumentException("Customer id cannot be zero, please check!");
            else if (string.IsNullOrEmpty(customer.FirstName))
                throw new ArgumentException($"Customer first name cannot be empty, please check id {customer.Id}!");
            else if (string.IsNullOrEmpty(customer.LastName))
                throw new ArgumentException($"Customer last name cannot be empty, please check id {customer.Id}!");
            else if (customer.Age < 18)
                throw new ArgumentException($"Customer age cannot be less than 18, please check id {customer.Id}!");
            else if (this._insertedCustomer.Exists(i => i.Id == customer.Id))
                throw new ArgumentException($"The customer already added, please check id {customer.Id}!");
        }

        /// <summary>
        /// Insert a customer to array
        /// Find a right index according to last name. if last name equal then check first name
        /// </summary>
        /// <param name="customer"></param>

        void InsertCustomer(CustomerDto customer)
        {
            int index = 0;

            foreach (var i in _insertedCustomer)
            {
                int compareLastName = String.Compare(i.LastName, customer.LastName, StringComparison.InvariantCulture);
                if (compareLastName < 0)
                    index++;
                else if (compareLastName == 0)
                {
                    int compareFirstName = String.Compare(i.FirstName, customer.FirstName, StringComparison.InvariantCulture);
                    if (compareFirstName < 0) index++;
                    else break;
                }
                else break;
            }

            _insertedCustomer.Insert(index, customer);
        }

        /// <summary>
        /// Save customer array to json file.
        /// </summary>

        void SaveCustomer()
        {
            string json = JsonSerializer.Serialize(_insertedCustomer);
            if (File.Exists(_fileName))
                File.Delete(_fileName);
            File.WriteAllText(_fileName, json);
        }
    }
}
