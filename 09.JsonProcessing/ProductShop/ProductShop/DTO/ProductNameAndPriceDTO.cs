
namespace ProductShop.DTO
{
    using Newtonsoft.Json;
    internal class ProductNameAndPriceDTO
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
    }
}