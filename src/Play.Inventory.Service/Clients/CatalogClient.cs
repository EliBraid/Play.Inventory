using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient
    {
        private readonly HttpClient client;

        public CatalogClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogAsync()
        {
            var items = await client.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");

            return items;
        }
    }
}