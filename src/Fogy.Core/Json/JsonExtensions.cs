using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Json
{
    public static class JsonExtensions
    {
        public static string ToJsonString(this object obj, bool camelCase = true, bool intented = false, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            var settings = new JsonSerializerSettings();

            if (camelCase)
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            else
                settings.ContractResolver = new DefaultContractResolver();

            if (intented)
                settings.Formatting = Formatting.Indented;

            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat });

            return ToJsonString(obj, settings);
        }

        public static string ToJsonString(object obj, JsonSerializerSettings settings)
        {
            return obj != null ?
                JsonConvert.SerializeObject(obj, settings)
                : default(string);
        }

        public static T FromJsonString<T>(this string value)
        {
            return value.FromJsonString<T>(new JsonSerializerSettings());
        }

        public static T FromJsonString<T>(this string value, JsonSerializerSettings settings)
        {
            return value != null ?
                JsonConvert.DeserializeObject<T>(value, settings)
                : default(T);
        }

        public static object FromJsonString(this string value, Type type, JsonSerializerSettings settings)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return value != null
                ? JsonConvert.DeserializeObject(value, type, settings)
                : null;
        }
    }
}
