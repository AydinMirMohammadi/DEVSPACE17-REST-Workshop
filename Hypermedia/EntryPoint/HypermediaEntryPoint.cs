using WebApiHypermediaExtensionsCore.Hypermedia;
using WebApiHypermediaExtensionsCore.Hypermedia.Attributes;

namespace CustomerDemo.Hypermedia.EntryPoint
{
    [HypermediaObject(Title = "Entry to the Rest API", Classes = new[] { "EntryPoint" })]
    public class HypermediaEntryPoint : HypermediaObject
    {
        public HypermediaEntryPoint()
        {
            
        }
    }
}
