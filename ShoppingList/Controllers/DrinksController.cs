using ShoppingList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ShoppingList.Controllers
{
    /// <summary>
    /// Controller for shopping drinks
    /// </summary>
    public class DrinksController : ApiController
    {
        /// <summary>
        /// List to store list
        /// </summary>
        public static List<Drink> drinks = new List<Drink>();

        const int PAGE_SIZE = 10;
        
        /// <summary>
        /// Retrieving drink list with paging
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public object GetAllDrinks(int page=0)
        {
            //Total number of drinks
            var totalCount = drinks.Count();

            //Calculate number of pages required
            var totalPages = Math.Ceiling((double)totalCount / PAGE_SIZE);


            var helper = new UrlHelper(Request);
            var prevUrl = page>0? helper.Link("DefaultApi", new { page = page - 1 }) : "";
            var nextUrl = page<totalPages-1? helper.Link("DefaultApi", new { page = page + 1 }) : "";

            //Generate result for requested page
            var results = drinks.Skip(PAGE_SIZE * page);


            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageUrl = prevUrl,
                NextPageUrl = nextUrl,
                Results = results
            };
        }

        /// <summary>
        /// Retrieving a drink by name
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public IHttpActionResult GetDrink(string Id)
        {
            //Find drink by name
            var drink = drinks.Find((d) => d.Name == Id);
            if (drink == null)
            {
                return NotFound();
            }
            return Ok(drink);
        }


        /// <summary>
        /// Posting a drink from request body
        /// </summary>
        /// <param name="drink"></param>
        /// <returns></returns>
        public IHttpActionResult PostDrink([FromBody]Drink drink)
        {
            //Insert if not already in list
            if ((drinks.Find((d) => d.Name == drink.Name)) == null)
            {
                drink.Id = Guid.NewGuid().ToString();
                drinks.Add(drink);
                return CreatedAtRoute("DefaultApi", new { id = drink.Name }, drink);
            }

            //Return conflict if already in list
            //return StatusCode(HttpStatusCode.Conflict);
            return Content(HttpStatusCode.Conflict, "Conflict found while adding " + drink.Name);
        }


        /// <summary>
        /// Removing a drink from list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(string id)
        {
            try
            {
                //Check if item exists
                if ((drinks.Find((d) => d.Name == id)) == null)
                    return Content(HttpStatusCode.NotFound, "Item not found");
                drinks.RemoveAll((d) => d.Name == id);
                return Content(HttpStatusCode.OK, "Item Deleted");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        /// <summary>
        /// Update drink quantity
        /// </summary>
        /// <param name="drink"></param>
        /// <returns></returns>
        [HttpPatch]
        [HttpPut]
        public IHttpActionResult PatchDrink([FromBody]Drink drink)
        {
            try
            {
                Drink drinkToUpdate = drinks.FirstOrDefault(d => d.Name == drink.Name);
                if (drinkToUpdate != null)
                {
                    drinkToUpdate.Quantity = drink.Quantity;
                    return Content(HttpStatusCode.OK, drink.Name + " quantity updated");
                }
                //Return not found if item does not exist
                return Content(HttpStatusCode.NotFound, "Item not found");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }

        }
    }
}
