using AspNetCoreWebAPI_ClientSide.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace AspNetCoreWebAPI_ClientSide.Controllers
{
    public class ProductController : Controller
    {
        

        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            List<Product> products = new List<Product>();
            using (var client = new HttpClient())
            {
                using(var response = await client.GetAsync("http://localhost:5048/api/Products"))
                {
                    string apiResponse = await  response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
             return View(products);
        }

        // GET: ProductController/Details/5
        
        public async Task<ActionResult> Details(int? id)
        {
            Product product = null;
            
            using (var client = new HttpClient())
            {
                string apiurl = "http://localhost:5048/api/Products/" + id;
                using (var response = await client.GetAsync(apiurl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }

            return View(product);
        }




        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product); // Return with validation errors
            }

            string json = JsonConvert.SerializeObject(new
            {
                name = product.Name,
                price = product.Price
            });

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5048/api/Products", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to the index page on success
                }
                else
                {
                    // Handle errors, maybe log them and show an error message to the user
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                    return View(product);
                }
            }
        }


        // GET: ProductController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                string apiurl = "http://localhost:5048/api/Products/" + id;
                using (var response = await client.GetAsync(apiurl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }

            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product); // Return with validation errors
            }

            string json = JsonConvert.SerializeObject(product);

            using (var client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"http://localhost:5048/api/Products/{product.ProductId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to the index page on success
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError(string.Empty, "Product ID mismatch.");
                    return View(product);
                }
                else
                {
                    // Handle other errors, maybe log them and show an error message to the user
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
                    return View(product);
                }
            }
        }


        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
