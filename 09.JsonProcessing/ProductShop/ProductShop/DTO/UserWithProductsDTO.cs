
namespace ProductShop.DTO
{
    using Newtonsoft.Json;
    internal class UserWithProductsDTO
    {
        [JsonProperty(PropertyName = "firstName")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int? Age { get; set; }

        [JsonProperty(PropertyName = "soldProducts")]
        public UserSoldProductDTO SoldProducts { get; set; }
    }
}