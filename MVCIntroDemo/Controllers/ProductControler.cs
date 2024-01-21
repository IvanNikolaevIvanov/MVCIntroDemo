using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models;
using System.Text;
using System.Text.Json;

namespace MVCIntroDemo.Controllers
{
    public class ProductController : Controller
    {

        private readonly IEnumerable<ProductViewModel> products 
            = new List<ProductViewModel>() 
            {
                new ProductViewModel()
                {
                    Id = 1,
                    Name = "Cheese",
                    Price = 7.00M
                },

                new ProductViewModel()
                {
                    Id = 2,
                    Name = "Ham",
                    Price = 5.50M
                },

                new ProductViewModel()
                {
                    Id = 3,
                    Name = "Bread",
                    Price = 1.50M
                }
            };

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult ById(int id)
        {
            var product = products.FirstOrDefault(products => products.Id == id);

            if (product == null)
            {
                TempData["Error"] = "No such product";

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return Json(products, options);
        }

        public IActionResult AllAsPlainText() 
        {
            
            return Content(GetAllProductsAsString());

        }

        public IActionResult AllAsTextFile() 
        {
            string content = GetAllProductsAsString();

            Response.Headers.Add(HeaderNames
                .ContentDisposition, @"attachment;filename=products.txt");

            return File(Encoding.UTF8.GetBytes(content), "text/plain");
        }

        public IActionResult Filtered(string? keyword)
        {
            if (keyword == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = products.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));

            return View(nameof(Index), model);
        }

        private string GetAllProductsAsString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var product in products)
            {
                sb.AppendLine($"Product {product.Id}: {product.Name} - {product.Price:f2} lv.");
            }

            return sb.ToString();
        }


    }
}
