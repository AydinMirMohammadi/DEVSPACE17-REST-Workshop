using WebApiHypermediaExtensionsCore.Hypermedia.Actions;

namespace CustomerDemo.Hypermedia.Customers
{
    public class FavoriteCustomer : IHypermediaActionParameter
    {
        public string CustomerLink { get; set; }
    }
}