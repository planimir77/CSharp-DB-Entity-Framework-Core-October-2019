
using System.Linq;

namespace ProductShop.DTO
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    internal class UserSoldProductDTO
    {
        [JsonProperty(PropertyName = "count")] 
        public int Count { get; set; } //=> this.Products.Count();//

        [JsonProperty(PropertyName = "products")]
        public IEnumerable<ProductNameAndPriceDTO> Products { get; set; }
    }
}