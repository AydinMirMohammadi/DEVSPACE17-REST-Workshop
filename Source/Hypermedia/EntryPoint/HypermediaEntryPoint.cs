using CustomerDemo.Hypermedia.Customers;
using WebApiHypermediaExtensionsCore.Hypermedia;
using WebApiHypermediaExtensionsCore.Hypermedia.Attributes;
using WebApiHypermediaExtensionsCore.Hypermedia.Links;

namespace CustomerDemo.Hypermedia.EntryPoint
{
    [HypermediaObject(Title = "Entry to the Rest API", Classes = new[] { "EntryPoint" })]
    public class HypermediaEntryPoint : HypermediaObject
    {
        public HypermediaEntryPoint(HypermediaCustomersRoot hypermediaCustomersRoot)
        {
            Links.Add(HypermediaLinks.EntryPoint.CustomersRoot, new HypermediaObjectReference(hypermediaCustomersRoot));
        }
    }
}
