using System.Net.Http.Json;
using System.Text.Json;

namespace MiProyecto003;

public class ProductService : IProductService
{
    private readonly HttpClient client;

    private readonly JsonSerializerOptions options; 

    public ProductService( HttpClient httpClient)
    {
        client = httpClient;
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

        public async Task<List<Product>?> GetProducts()
        {
            var response = await client.GetAsync("v1/products");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<List<Product>>(content, options);
            }
            else
            {
                // Manejar el error de la llamada a la API
                new Exception("No se pudieron obtener los productos.");
            }
            return JsonSerializer.Deserialize<List<Product>>(content, options);
        }

        public async Task Add(Product product)
        {
            var response = await client.PostAsync("v1/products", JsonContent.Create(product));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
        }

        public async Task DeleteProduct(int productId)
        {
            var response = await client.DeleteAsync($"v1/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
            // Manejar el error de la llamada a la API para eliminar el producto
            throw new Exception("No se pudo eliminar el producto.");
            }

        }
}

 public interface IProductService
    {
        Task<List<Product>?> GetProducts();

        Task Add(Product product);

        Task DeleteProduct(int productId);
    }