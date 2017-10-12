using System;
using System.Threading.Tasks;
using CustomerDemo.Domain.Customer;
using Hypermedia.Util;
using WebApiHypermediaExtensionsCore.Hypermedia;
using WebApiHypermediaExtensionsCore.Hypermedia.Actions;
using WebApiHypermediaExtensionsCore.Hypermedia.Attributes;
using WebApiHypermediaExtensionsCore.Hypermedia.Links;

namespace CustomerDemo.Hypermedia.Customers
{
    [HypermediaObject(Title = "The Customers API", Classes = new[] { "CustomersRoot" })]
    public class HypermediaCustomersRoot : HypermediaObject
    {
        private readonly ICustomerRepository customerRepository;

        // Add actions:
        // Each ActionType must be unique and a corresponding route must exist so the formatter can look it up.
        // See the CustomersRootController.
        [HypermediaAction(Name = "CreateCustomer", Title = "Request creation of a new Customer.")]
        public HypermediaFunction<CreateCustomerParameters, Task<Customer>> CreateCustomerAction { get; private set; }

        [HypermediaAction(Name = "CreateQuery", Title = "Query the Customers collection.")]
        public HypermediaAction<CustomerQuery> CreateQueryAction { get; private set; }

        public HypermediaCustomersRoot(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;

            CreateQueryAction = new HypermediaAction<CustomerQuery>(CanNewQuery);
            CreateCustomerAction = new HypermediaFunction<CreateCustomerParameters, Task<Customer>>(CanCreateCustomer, DoCreateCustomer);

            // Add Links:
            var allQuery = new CustomerQuery();
            Links.Add(DefaultHypermediaRelations.Queries.All, new HypermediaObjectQueryReference(typeof(HypermediaCustomerQueryResult), allQuery));

        }

        // Will be called to determine if tis action is available at the moment/current state.
        private bool CanNewQuery()
        {
            return true;
        }

        private async Task<Customer> DoCreateCustomer(CreateCustomerParameters arg)
        {
            var customer = CustomerService.CreateRandomCustomer();
            customer.Name = arg.Name;
            await customerRepository.AddEntityAsync(customer);

            return customer;
        }

        private bool CanCreateCustomer()
        {
            return true;
        }

    }
}
