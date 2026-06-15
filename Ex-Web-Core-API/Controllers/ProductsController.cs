using System.ComponentModel.DataAnnotations;
using Ex_Web_Core_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ex_Web_Core_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = [];
    private static int _nextId = 1;
    private static readonly object SyncRoot = new();

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        lock (SyncRoot)
        {
            product.Id = _nextId++;
            Products.Add(product);
        }

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(
        [FromRoute]
        [Range(1, int.MaxValue, ErrorMessage = "ID phải là số nguyên dương.")]
        int id)
    {
        Product? product;

        lock (SyncRoot)
        {
            product = Products.FirstOrDefault(item => item.Id == id);
        }

        if (product is null)
        {
            return NotFound(new
            {
                message = $"Không tìm thấy sản phẩm có ID {id}."
            });
        }

        return Ok(product);
    }
}
