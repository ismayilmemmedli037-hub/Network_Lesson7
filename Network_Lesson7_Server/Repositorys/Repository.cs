using Network_Lesson7_Shared.Contexts;
using Network_Lesson7_Shared.Models;
namespace Network_Lesson7_Server.Repositorys;

public class Repository : IRepository
{
    public async Task<bool> Add(Product product)
    {
        using var data = new AppDataContext();
        await data.Products.AddAsync(product);
        return await data.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(int id)
    {
        using var data = new AppDataContext();
        var p = data.Products.FirstOrDefault(x => x.Id == id);
        if (p == null)
        {
            return false;
        }
        data.Products.Remove(p);
        return await data.SaveChangesAsync() > 0;
    }

    public List<Product>? GetAll()
    {
        using var data = new AppDataContext();
        return data.Products.ToList();
    }

    public Product? GetById(int id)
    {
        using var data = new AppDataContext();
        return data.Products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<bool> Update(Product product)
    {
        using var data = new AppDataContext();
        var old = data.Products.FirstOrDefault(p => p.Id == product.Id);
        if (old == null)
        {
            return false;
        }
        old.Name = product.Name;
        old.Price = product.Price;
        old.Category = product.Category;
        return await data.SaveChangesAsync() > 0;
    }
}
