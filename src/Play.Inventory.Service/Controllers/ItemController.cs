using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemController: ControllerBase
    {
        private readonly IRepository<InventoryItem> itemRepository;       
        private readonly CatalogClient catalogClient;
        public ItemController(IRepository<InventoryItem> itemRepository, CatalogClient catalogClient)
        {
            this.itemRepository = itemRepository;
            this.catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if(userId == Guid.Empty){
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogAsync();
            var inventoryItemEntities = await itemRepository.GetAllAsync(item=>
            item.UserId == userId);
            var inventoryItemDtos = inventoryItemEntities.Select(inventory=>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventory.CatelogItemId);
                return inventory.ASDto(catalogItem.Name,catalogItem.Description);
            });
            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemDto){
            var inventoryItem = await itemRepository.GetAsync(
                item=>
                item.UserId == grantItemDto.UserId && 
            item.CatelogItemId == grantItemDto.CatalogItemId);
            
            if(inventoryItem == null){
                inventoryItem = new InventoryItem{
                    CatelogItemId = grantItemDto.CatalogItemId,
                    UserId = grantItemDto.UserId,
                    Quantity = grantItemDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await itemRepository.CreateAsync(inventoryItem);
            }else{
                inventoryItem.Quantity += grantItemDto.Quantity;
                await itemRepository.UpdateAsync(inventoryItem); 
                }
                return Ok();           
        }
    }
}
