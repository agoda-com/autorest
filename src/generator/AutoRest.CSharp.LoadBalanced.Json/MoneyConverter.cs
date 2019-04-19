using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoRest.CSharp.LoadBalanced.Json
{
    public class MoneyConverter : JsonConverterBase<decimal, string>
    {
        private readonly MoneyConverterOptions? _options;

        public MoneyConverter()
        {
        }

        public MoneyConverter(MoneyConverterOptions options)
        {
            _options = options;
        }

        protected override bool TryParse(decimal model, out string dto)
        {
            dto = model.ToString();
            return true;
        }

        protected override bool TryParse(string dto, out decimal model)
        {
            return Decimal.TryParse(dto, out model);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null && IsNullable)
            {
                JToken.FromObject(null).WriteTo(writer);
                return;
            }
            
            var moneyValue = 0m;
            if (value is decimal)
            {
                moneyValue = (decimal)value;
            }
            else
            {
                decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out moneyValue);
            }

            var typedValue = SendAsText ? (object)ConvertDecimalToText(moneyValue) : moneyValue;
            JToken.FromObject(typedValue).WriteTo(writer);
        }

        public static string ConvertDecimalToText(decimal value)
        {
            return value.ToString();
        }

        public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isText = objectType == typeof(string);
            var isNullable = isText || Nullable.GetUnderlyingType(objectType) != null;
            var defaultValue = isNullable ? null : (isText ? (object) "0" : 0m);

            JToken token = JToken.Load(reader);
            if (token == null)
            {
                return defaultValue;
            }

            var moneyValue = 0m;
            if (decimal.TryParse(token.ToString() ?? "", NumberStyles.Any, CultureInfo.InvariantCulture, out moneyValue))
            {
                return isText ? (object)ConvertDecimalToText(moneyValue) : moneyValue;
            }
            else
            {
                return defaultValue;
            }
        }

        protected bool SendAsText => _options?.HasFlag(MoneyConverterOptions.SendAsText) ?? false;
        protected bool IsNullable => _options?.HasFlag(MoneyConverterOptions.IsNullable) ?? false;

        public sealed override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal?) ||
                   objectType == typeof(decimal);
        }
    }
}


