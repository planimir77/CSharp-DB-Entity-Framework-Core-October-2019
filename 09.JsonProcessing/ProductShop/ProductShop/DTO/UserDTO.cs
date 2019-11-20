namespace ProductShop.DTO
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    internal class UserDTO
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "soldProducts")]
        public ICollection<SoldProductsDTO> SoldProducts { get; set; }
    }
}