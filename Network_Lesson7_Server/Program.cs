using Network_Lesson7_Server.Repositorys;
using Network_Lesson7_Shared.Models;
using System.Net;
using System.Text;
using System.Text.Json;

IRepository repo = new PartnerRepository(new Repository());
HttpListener listener = new HttpListener();

listener.Prefixes.Add("http://localhost:3000/");
listener.Start();

Console.WriteLine("Server isleyir: http://localhost:3000/");

while (true)
{
    HttpListenerContext context = await listener.GetContextAsync();

    try
    {
        await HandleRequest(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("SERVER XETASI:");
        Console.WriteLine(ex.ToString());

        try
        {
            context.Response.StatusCode = 500;
            context.Response.Close();
        }
        catch
        {
        }
    }
}

async Task HandleRequest(HttpListenerContext context)
{
    HttpListenerRequest request = context.Request;
    string path = request.Url!.AbsolutePath.Trim('/');
    string[] parts = path.Split('/');

    if (parts[0] != "products")
    {
        SendResponse(context, 404, "Bele unvan yoxdur");
        return;
    }

    int id = 0;

    if (parts.Length > 1)
    {
        int.TryParse(parts[1], out id);
    }

    if (request.HttpMethod == "GET")
    {
        if (id == 0)
        {
            SendResponse(context, 200, repo.GetAll());
        }
        else
        {
            Product? product = repo.GetById(id);

            if (product == null)
            {
                SendResponse(context, 404, "Mehsul yoxdur");
            }
            else
            {
                SendResponse(context, 200, product);
            }
        }
    }
    else if (request.HttpMethod == "POST")
    {
        try
        {
            Product product = ReadProduct(request);

            Console.WriteLine("Ad: " + product.Name);
            Console.WriteLine("Qiymet: " + product.Price);
            Console.WriteLine("Kateqoriya: " + product.Category);

            bool result = await repo.Add(product);

            if (result)
            {
                SendResponse(context, 201, "Mehsul elave edildi");
            }
            else
            {
                SendResponse(context, 400, "Mehsul elave edilmedi");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("POST XETASI:");
            Console.WriteLine(ex.ToString());

            SendResponse(context, 500, "Server xetasi");
        }
    }
    else if (request.HttpMethod == "PUT")
    {
        Product product = ReadProduct(request);
        bool result = await repo.Update(product);

        if (result)
        {
            SendResponse(context, 200, "Mehsul yenilendi");
        }
        else
        {
            SendResponse(context, 404, "Mehsul yoxdur");
        }
    }
    else if (request.HttpMethod == "DELETE")
    {
        if (id == 0)
        {
            SendResponse(context, 400, "Mehsulun Id qeyd edilmelidir");
        }
        else
        {
            bool result = await repo.Delete(id);

            if (result)
            {
                SendResponse(context, 200, "Mehsul silindi");
            }
            else
            {
                SendResponse(context, 404, "Mehsul yoxdur");
            }
        }
    }
    else
    {
        SendResponse(context, 405, "Emeliyyata icaze verilmir");
    }
}

Product ReadProduct(HttpListenerRequest request)
{
    StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
    string json = reader.ReadToEnd();

    Product product = JsonSerializer.Deserialize<Product>(
        json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

    return product;
}

void SendResponse(HttpListenerContext context, int statusCode, object? data)
{
    string json = JsonSerializer.Serialize(data);
    byte[] bytes = Encoding.UTF8.GetBytes(json);

    context.Response.StatusCode = statusCode;
    context.Response.ContentType = "application/json; charset=utf-8";
    context.Response.ContentLength64 = bytes.Length;
    context.Response.OutputStream.Write(bytes, 0, bytes.Length);
    context.Response.Close();
}