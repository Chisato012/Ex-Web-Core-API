using System.ComponentModel.DataAnnotations;

namespace Ex_Web_Core_API.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MinLength(3, ErrorMessage = "Tên sản phẩm phải có ít nhất 3 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal),
        "0.1",
        "999999999",
        ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    public decimal Price { get; set; }
}
