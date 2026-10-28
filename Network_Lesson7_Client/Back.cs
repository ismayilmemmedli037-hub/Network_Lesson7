using Network_Lesson7_Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Network_Lesson7_Client;

public class Back
{
    static HttpClient client = new HttpClient();

    public static async Task Show()
    {
        client.BaseAddress = new Uri("http://localhost:3000/");

        string[] methods = { "Add", "Update", "GetAll", "GetById", "Delete", "Exit" };

        while (true)
        {
            short choice = Network_Lesson7_Client.Front.ShowMenu(methods, "================ METHODS ================");

            if (choice == 0)
            {
                await AddProduct();
            }
            else if (choice == 1)
            {
                await UpdateProduct();
            }
            else if (choice == 2)
            {
                await GetAllProducts();
            }
            else if (choice == 3)
            {
                await GetProductById();
            }
            else if (choice == 4)
            {
                await DeleteProduct();
            }
            else if (choice == 5)
            {
                return;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Davam etmek ucun Enter basin...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }

    static void ShowTitle(string title)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    static async Task AddProduct()
    {
        ShowTitle("================= MEHSUL ELAVE ET =================");
        Product product = ReadProduct(false);
        HttpResponseMessage response = await client.PostAsJsonAsync("products", product);

        string message = await response.Content.ReadAsStringAsync();
        Console.WriteLine((int)response.StatusCode + ": " + message);
    }

    static async Task UpdateProduct()
    {
        ShowTitle("================= MEHSULU YENILE =================");
        Product product = ReadProduct(true);
        HttpResponseMessage response = await client.PutAsJsonAsync("products", product);

        string message = await response.Content.ReadAsStringAsync();
        Console.WriteLine((int)response.StatusCode + ": " + message);
    }

    static async Task GetAllProducts()
    {
        ShowTitle("================= MEHSULLAR =================");
        List<Product>? products = await client.GetFromJsonAsync<List<Product>>("products");

        if (products == null || products.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Siyahi bosdur!");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (Product p in products)
        {
            Console.WriteLine(p.Id + " | " + p.Name + " | " + p.Price + " | " + p.Category);
        }
        Console.ResetColor();
    }

    static async Task GetProductById()
    {
        ShowTitle("================= MEHSULU ID ILE TAP =================");

        Console.Write("Id: ");
        int id = int.Parse(Console.ReadLine());

        HttpResponseMessage response = await client.GetAsync("products/" + id);

        if (response.IsSuccessStatusCode)
        {
            Product? p = await response.Content.ReadFromJsonAsync<Product>();
            Console.WriteLine(p.Id + " | " + p.Name + " | " + p.Price + " | " + p.Category);
        }
        else
        {
            Console.WriteLine("Tapilmadi");
        }
    }

    static async Task DeleteProduct()
    {
        ShowTitle("================= MEHSULU SIL =================");

        Console.Write("Id: ");
        int id = int.Parse(Console.ReadLine());

        HttpResponseMessage response = await client.DeleteAsync("products/" + id);

        string message = await response.Content.ReadAsStringAsync();
        Console.WriteLine((int)response.StatusCode + ": " + message);
    }

    public static Product ReadProduct(bool withId)
    {
        Product product = new Product();

        if (withId == true)
        {
            Console.Write("Id: ");
            product.Id = int.Parse(Console.ReadLine());
        }
        Console.Write("Ad: ");
        product.Name = Console.ReadLine();
        Console.Write("Qiymet: ");
        product.Price = decimal.Parse(Console.ReadLine());
        Console.Write("Kateqoriya: ");
        product.Category = Console.ReadLine();

        return product;
    }
}