namespace Exhibition.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public static class JsonExtension
    {
        public static T DeserializeToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
                return default(T);
            }
        }
        public static Dictionary<string, string> DeserializeToDictionary(this string json)
        {
            return DeserializeToObject<Dictionary<string, string>>(json);
        }

        public static string SerializeToJson<T>(this T data)
        {
            try
            {
                return JsonConvert.SerializeObject(data, Formatting.None);
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex);
                return string.Empty;
            }
        }
        public static T DeserializeFromStream<T>(this Stream stream) where T : class
        {
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(jsonTextReader) as T;
                }
            }
        }
    }
}