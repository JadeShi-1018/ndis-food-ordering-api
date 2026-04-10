using System.ComponentModel.DataAnnotations.Schema;

namespace NDISServiceAPI.DTO.Item
{
    public class CreateItemRequestDto
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ProviderServiceId { get; set; }
    }
}
