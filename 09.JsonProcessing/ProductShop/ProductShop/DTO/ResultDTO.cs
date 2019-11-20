
using Newtonsoft.Json;

namespace ProductShop.DTO
{
    using System.Collections.Generic;
    internal class ResultDTO
    {

        [JsonProperty(PropertyName = "usersCount")]
        public int UsersCount { get; set; }


        [JsonProperty(PropertyName = "users")]
        public List<UserWithProductsDTO> Users { get; set; }
    }
}