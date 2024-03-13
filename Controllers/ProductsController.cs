using DemoPilotoV1.BDD;
using DemoPilotoV1.DTOS;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoPilotoV1.Controllers
{
    public class ProductsController : Controller
    {

        private readonly BaseDeDatos _context;
        private readonly RepoProductos _repoProducto;

        public ProductsController(BaseDeDatos context)
        {
            _context = context;
            _repoProducto = new RepoProductos(context); // Asegúrate de inicializar _repoProducto
        }

        public IActionResult ObtenerProductos()
        {
            var productos = _repoProducto.ObtenerTodosLosProductos();
            return Ok(productos);
        }

        [HttpGet("Productos/{id}/Imagen")]
        public IActionResult ObtenerImagenDeProducto(int id)
        {
            var producto = _repoProducto.ObtenerProductoPorId(id);

            if (producto == null || producto.ImageData == null || producto.ImageData.Length == 0)
                return NotFound(); // Puedes devolver otra respuesta si la imagen no está presente

            return File(producto.ImageData, "image/jpeg"); // Ajusta el tipo MIME según el formato de tus imágenes
        }

        [HttpPost("crear")]
        [SwaggerOperation("Crea un nuevo producto")]
        [SwaggerResponse(StatusCodes.Status200OK, "Producto creado exitosamente")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos del producto no válidos")]
        public IActionResult CrearProducto([FromForm] ProductoDto nuevoProductoDto)
        {
            try
            {
                if (nuevoProductoDto == null || nuevoProductoDto.ImagenData == null)
                {
                    return BadRequest("Datos del producto no válidos");
                }

                byte[] imagenDataBytes;

                using (var memoryStream = new MemoryStream())
                {
                    nuevoProductoDto.ImagenData.CopyTo(memoryStream);
                    imagenDataBytes = memoryStream.ToArray();
                }

                _repoProducto.GuardarProducto(nuevoProductoDto.Name, nuevoProductoDto.Price, imagenDataBytes);

                return Ok(new { Message = "Producto creado exitosamente" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }



        [HttpGet("Productos")]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // ----------------- * -----------


        // GET: ProductsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
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
