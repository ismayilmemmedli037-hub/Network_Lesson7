using Network_Lesson7_Shared.Models;
namespace Network_Lesson7_Server.Repositorys;

public interface IRepository
{
    Product? GetById(int id);
    List<Product>? GetAll();
    Task<bool> Add(Product product);
    Task<bool> Update(Product product);
    Task<bool> Delete(int id);
}
