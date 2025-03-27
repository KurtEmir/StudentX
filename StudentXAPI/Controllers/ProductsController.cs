using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;


namespace ProductsAPI.Controllers
{

    // localhost:5000/api/products
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductsContext _context;


        public ProductsController(ProductsContext context)
        {
            _context = context;
        }


    // localhost:5000/api/products => Get
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.Where(i => i.IsActive).Select( p => 
            new ProductDTO{
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price
             }).ToListAsync();
            if(products == null) 
            {
                return NotFound();
            }
            
            return Ok(products);
        }

    // localhost:5000/api/products/1 => Get

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id)  //IActionResult action'ın http status code geri döndürmesine yarar
        {
            if(id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
            .AsNoTracking()
            .Where(i => i.IsActive)
            .Select(p => ProductToDTO(p))
            .FirstOrDefaultAsync(p => p.ProductId == id);

            if(product == null) 
            {
                return NotFound();
            }

            return Ok(product);     //200 durum kodu ile product nesnesini döndürür

        }

        // ürün eklerken neden id sini yazmıyoruz.
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product); //201 durum kodu gönderiyoruz
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if(id != entity.ProductId) 
            {
                return BadRequest();
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if(product == null)
            {
                return NotFound();
            }

            product.ProductName = entity.ProductName;
            product.Price = entity.Price;
            product.IsActive = entity.IsActive;

            try
            {
               await _context.SaveChangesAsync();     
            }
            catch(Exception)
            {
                return NotFound();
            }

            return NoContent(); //204 durum koduna karşılık gelir
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct (int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private static ProductDTO ProductToDTO (Product p)
        {
            return new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price
            };
        }


    }
}

