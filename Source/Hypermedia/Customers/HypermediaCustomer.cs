using CustomerDemo.Domain.Customer;
using WebApiHypermediaExtensionsCore.Exceptions;
using WebApiHypermediaExtensionsCore.Hypermedia;
using WebApiHypermediaExtensionsCore.Hypermedia.Attributes;

namespace CustomerDemo.Hypermedia.Customers
{
    [HypermediaObject(Title = "A Customer", Classes = new[] { "Customer" })]
    public class HypermediaCustomer : HypermediaObject
    {
        private readonly Customer customer;

        // Hides the Property so it will not be pressent in the Hypermedia.
        [FormatterIgnoreHypermediaProperty]
        public int Id { get; set; }

        // Assigns an alternative name, so this stays constant even if property is renamed
        [HypermediaProperty(Name = "FullName")]
        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public bool IsFavorite { get; set; }

        public HypermediaCustomer(Customer customer)
        {
            this.customer = customer;

            Name = customer.Name;
            Id = customer.Id;
            Age = customer.Age;
            IsFavorite = customer.IsFavorite;
            Address = customer.Address;
        }

       
    }
}