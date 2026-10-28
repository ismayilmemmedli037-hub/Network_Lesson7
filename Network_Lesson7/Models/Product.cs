using Network_Lesson7_Shared.BaseEntity;
namespace Network_Lesson7_Shared.Models;

public class Product : Entity
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Category { get; set; }

}
