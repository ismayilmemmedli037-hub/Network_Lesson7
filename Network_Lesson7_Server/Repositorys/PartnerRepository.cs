using Microsoft.Extensions.Caching.Memory;
using Network_Lesson7_Shared.Models;
namespace Network_Lesson7_Server.Repositorys;

public class PartnerRepository : IRepository
{
    private readonly IRepository _repository;
    private readonly IMemoryCache _cache;

    public PartnerRepository(IRepository repository)
    {
        _repository = repository;
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    public Product? GetById(int id)
    {
        string key = $"product:{id}";

        if (_cache.TryGetValue(key, out Product? product))
        {
            Console.WriteLine("Melumat Cache goturuldu");
            return product;
        }

        product = _repository.GetById(id);

        if (product != null)
        {
            _cache.Set(key, product, TimeSpan.FromMinutes(10));
        }

        Console.WriteLine("Melumat Repository goturuldu");

        return product;
    }

    public List<Product>? GetAll()
    {
        string key = "products:all";

        if (_cache.TryGetValue(key, out List<Product>? products))
        {
            Console.WriteLine("Butun mehsullar Cache goturuldu");
            return products;
        }

        products = _repository.GetAll();

        if (products != null)
        {
            _cache.Set(key, products, TimeSpan.FromMinutes(10));
        }

        Console.WriteLine("Butun mehsullar Repository goturuldu");

        return products;
    }

    public async Task<bool> Add(Product product)
    {
        bool result = await _repository.Add(product);

        if (result)
        {
            _cache.Remove("products:all");
            Console.WriteLine("Mehsul elave edildi");
        }

        return result;
    }

    public async Task<bool> Update(Product product)
    {
        bool result = await _repository.Update(product);

        if (result)
        {
            _cache.Remove($"product:{product.Id}");
            _cache.Remove("products:all");

            Console.WriteLine("Mehsul yenilendi");
        }

        return result;
    }

    public async Task<bool> Delete(int id)
    {
        bool result = await _repository.Delete(id);

        if (result)
        {
            _cache.Remove($"product:{id}");
            _cache.Remove("products:all");

            Console.WriteLine("Mehsul silindi");
        }

        return result;
    }
}