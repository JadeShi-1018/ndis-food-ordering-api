using System.ComponentModel.DataAnnotations.Schema;

namespace NDISServiceAPI.DTO.Item
{
    public class ItemResponseDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ProviderServiceId { get; set; }
    }
}
