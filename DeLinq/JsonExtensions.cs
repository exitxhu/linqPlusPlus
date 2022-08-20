using JsonFormatterPlus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace linqPlusPlus
{
    public static class JsonExtensions
    {
        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }

        public static string ToJson(this object model, dynamic jsonSerializerSettings = null)
        {
            //return model is not null ? JsonConvert.SerializeObject(model)?.Replace(@"\", "") : null;
            return model is not null ? JsonConvert.SerializeObject(model, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            }) : null;
        }
        public static string XmlToJson(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;
            if (xml.StartsWith("["))
            {
                xml = xml.Replace("[", "");
            }
            if (xml.EndsWith("]"))
            {
                xml = xml.Replace("]", "");
            }

            xml = xml.Replace("'>", "'/>");
            xml = xml.Replace("/>", "'>");
            xml = xml.Replace(">", "'/>");
            xml = xml.Replace(">\"\"", ">");
            xml = xml.Replace("''''", "''");
            var doc = new XmlDocument();
            doc.LoadXml(@xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
        public static string ToJson(object model, JsonSerializerSettings config)
        {
            return model is not null ? JsonConvert.SerializeObject(model, config) : null;
        }
        public static T DeserializeJson<T>(string json, dynamic jsonSerializerSettings = null)
        {
            //json = "['qasdw','wds','waed','salam']";
            if (string.IsNullOrWhiteSpace(json)) return default(T);
            if (!IsValidJson(json))
            {
                //json = json.Replace("\"", "");
                //json = json.Substring(1, json.Length - 2);
                return ((T)Convert.ChangeType(json, typeof(T)));
            }

            if (jsonSerializerSettings is not null)
                return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings as JsonSerializerSettings);
            var res = JsonConvert.DeserializeObject<T>(json);
            return res;
        }
        public static object DeserializeJson(this string json, Type type, dynamic jsonSerializerSettings = null)
        {
            //json = "['qasdw','wds','waed','salam']";
            if (string.IsNullOrWhiteSpace(json)) return default(object);
            if (!IsValidJson(json))
            {
                //json = json.Replace("\"", "");
                //json = json.Substring(1, json.Length - 2);
                return Convert.ChangeType(json, type);
            }

            if (jsonSerializerSettings is not null)
                return JsonConvert.DeserializeObject(json, type, jsonSerializerSettings as JsonSerializerSettings);
            var res = JsonConvert.DeserializeObject(json, type);
            return res;
        }
        public static T CleanJson<T>(string jsonData)
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                var json = jsonData.Replace("\t", "").Replace("\r\n", "");
                var loop = true;
                do
                {
                    try
                    {
                        var m = JsonConvert.DeserializeObject<T>(json);
                        loop = false;
                    }
                    catch (JsonReaderException ex)
                    {
                        var position = ex.LinePosition;
                        var invalidChar = json.Substring(position - 2, 2);
                        invalidChar = invalidChar.Replace("\"", "'");
                        json = $"{json.Substring(0, position - 1)}{invalidChar}{json.Substring(position)}";
                    }
                } while (loop);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }
        public static bool IsEmptyOrNull(string json)
        {
            if (string.IsNullOrEmpty(json))
                return true;
            try
            {
                var output = JsonConvert.DeserializeObject<dynamic>(json);
                if (((JObject)output).Count == 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return true;
            }

        }
        public static string ExtractJsonResponse(Stream response)
        {
            string json;
            using (var outStream = new MemoryStream())
            using (var zipStream = new GZipStream(response,
                CompressionMode.Decompress))
            {
                zipStream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }
            return json;
        }
        public static string SerializeStream<T>(T obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }
        public static string Format(string json)
        {
            return JsonFormatter.Format(json);
        }
        public static string Minify(string json)
        {
            if (json is not null)
                return JsonFormatter.Minify(json);
            else return null;
        }

    }

}
