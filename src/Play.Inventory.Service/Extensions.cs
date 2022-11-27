using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static InventoryItemDto ASDto(this InventoryItem item,string name,string description)
        {
            return new InventoryItemDto(item.CatelogItemId,name,description,item.Quantity,item.AcquiredDate);
        }
    }
}