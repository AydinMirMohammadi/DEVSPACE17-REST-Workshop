using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CustomerDemo.Domain.Customer;
using CustomerDemo.Hypermedia.Customers;
using CustomerDemo.Util;
using Microsoft.AspNetCore.Mvc;
using WebApiHypermediaExtensionsCore.ErrorHandling;
using WebApiHypermediaExtensionsCore.Exceptions;
using WebApiHypermediaExtensionsCore.WebApi;
using WebApiHypermediaExtensionsCore.WebApi.AttributedRoutes;
using WebApiHypermediaExtensionsCore.WebApi.ExtensionMethods;

namespace CustomerDemo.Controllers.Customers
{
    [Route("Customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        // Route to the HypermediaCustomer. References to HypermediaCustomer type will be resolved to this route.
        // This RouteTemplate also contains a key, so a RouteKeyProducer is required.
        [HttpGetHypermediaObject("{key:int}", typeof(HypermediaCustomer), typeof(CustomerRouteKeyProducer))]
        public async Task<ActionResult> GetEntity(int key)
        {
            try
            {
                var customer = await customerRepository.GetEnitityByKeyAsync(key);
                var result = new HypermediaCustomer(customer);
                return Ok(result);
            }
            catch (EntityNotFoundException)
            {
                return this.Problem(ProblemJsonBuilder.CreateEntityNotFound());
            }
        }

        
    }
}
