using WebApiHypermediaExtensionsCore.Hypermedia.Actions;

namespace CustomerDemo.Hypermedia.Customers
{
    public class CreateCustomerParameters : IHypermediaActionParameter
    {
        public string Name { get; set; }
    }
}