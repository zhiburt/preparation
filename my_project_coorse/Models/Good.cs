using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace preparation.Models
{
    public class Good : IProduct
    {
        public decimal Price { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<Supplier>))]
        public ISupplier Supplier { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<Preparation>))]
        public IGood Product { get; set; }
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            var good = obj as Good;
            return good != null &&
                   Price == good.Price &&
                   EqualityComparer<ISupplier>.Default.Equals(Supplier, good.Supplier) &&
                   EqualityComparer<IGood>.Default.Equals(Product, good.Product);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Price, Supplier, Product);
        }
    }

    public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            //assume we can convert to anything for now
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
}
