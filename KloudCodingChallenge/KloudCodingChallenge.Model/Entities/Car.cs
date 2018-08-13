
using KloudCodingChallenge.Model.Interfaces;
using Newtonsoft.Json;

namespace KloudCodingChallenge.Model.Entities
{
    public class Car : ICar
    {
        public Car()
        {
        }
        [JsonProperty("brand")]
        public string Brand { get ;set; }
        [JsonProperty("colour")]
        public string Color { get ;set; }
    }
}
