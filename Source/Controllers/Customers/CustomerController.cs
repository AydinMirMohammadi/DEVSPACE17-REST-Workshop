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

        [HttpPostHypermediaAction("MyFavoriteCustomers", typeof(HypermediaActionCustomerMarkAsFavorite))]
        public async Task<ActionResult> MarkAsFovoriteAction([SingleParameterBinder(typeof(FavoriteCustomer))]  FavoriteCustomer favoriteCustomer)
        {
            try
            {
                var id = ExtractIdFromCustomerUri(favoriteCustomer.CustomerLink);

                var customer = await customerRepository.GetEnitityByKeyAsync(id);
                var hypermediaCustomer = new HypermediaCustomer(customer);
                hypermediaCustomer.MarkAsFavoriteAction.Execute(favoriteCustomer);
                return Ok();
            }
            catch (EntityNotFoundException)
            {
                return this.Problem(ProblemJsonBuilder.CreateEntityNotFound());
            }
            catch (InvalidLinkException e)
            {
                var problem = new ProblemJson()
                {
                    Title = $"Can not use provided object of type '{typeof(FavoriteCustomer)}'",
                    Detail = e.Message,
                    ProblemType = "WebApiHypermediaExtensionsCore.Hypermedia.BadActionParameter",
                    StatusCode = 422 // Unprocessable Entity
                };
                return this.UnprocessableEntity(problem);
            }
            catch (CanNotExecuteActionException)
            {
                return this.CanNotExecute();
            }

        }

        private int ExtractIdFromCustomerUri(string favoriteCustomerCustomerLink)
        {
            if (string.IsNullOrWhiteSpace(favoriteCustomerCustomerLink))
            {
                throw new InvalidLinkException($"Provided Link is empty '{favoriteCustomerCustomerLink}'");
            }
            var lastSegment = favoriteCustomerCustomerLink.Split('/').Last();

            try
            {
                return Convert.ToInt16(lastSegment);

            }
            catch (Exception)
            {
                throw new InvalidLinkException($"Provided Link is invalid '{favoriteCustomerCustomerLink}', provide propper self link.");
            }
        }


    }
}
