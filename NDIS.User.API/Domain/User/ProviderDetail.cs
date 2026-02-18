using System.ComponentModel.DataAnnotations;

namespace NDIS.User.API.Domain.User
{
    public class ProviderDetail
    {
        [Required]
        public string ProviderDetailId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]

        public string ProviderEmail { get; set; }
        [Required]
        public string ProviderPhoneNumber { get; set; }
        [Required]
        public int ABN { get; set; }
        [Required]
        public string AddressLine { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ProviderQualification { get; set; }

        //navigate properties
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }



    }
}
