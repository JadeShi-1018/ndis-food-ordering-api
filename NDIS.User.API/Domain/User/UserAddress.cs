namespace NDIS.User.API.Domain.User
{
    public class UserAddress
    {
        public string UserAddressId { get; set; }


        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }

        // navigate property
       
        public User User { get; set; }
    }
}
