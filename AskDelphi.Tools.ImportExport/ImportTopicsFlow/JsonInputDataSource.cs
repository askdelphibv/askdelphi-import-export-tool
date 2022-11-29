using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskDelphi.Tools.ImportExport.ImportTopicsFlow
{
    public class JsonInputDataSource : IEnumerable<Dictionary<string, string>>
    {
        readonly string filename;

        public JsonInputDataSource(string filename)
        {
            this.filename = filename;
        }

        public IEnumerator<Dictionary<string, string>> GetEnumerator()
        {
            return new JsonInputDataSourceEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new JsonInputDataSourceEnumerator(this);

        }

        private class JsonInputDataSourceEnumerator : IEnumerator<Dictionary<string, string>>
        {
            private JsonInputDataSource jsonInputDataSource;
            private readonly JArray json;
            private int index = -1;

            public JsonInputDataSourceEnumerator(JsonInputDataSource jsonInputDataSource)
            {
                this.jsonInputDataSource = jsonInputDataSource;

                using (var jsonStreamReader = File.OpenText(jsonInputDataSource.filename))
                {
                    using (var jsonTextReader = new JsonTextReader(jsonStreamReader))
                    {
                        JArray jsonObject = (JArray)JToken.ReadFrom(jsonTextReader);
                        if (jsonObject.Type != JTokenType.Array)
                        {
                            throw new InvalidOperationException($"The JSON data file should be an array of objects.");
                        }
                        json = jsonObject;
                    }
                }
                    
            }

            public Dictionary<string, string> Current => GetCurrent();

            object IEnumerator.Current => GetCurrent();

            private Dictionary<string, string> GetCurrent()
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                JToken token = json[index];
                result["topicGuid"] = token["topicGuid"]?.ToObject<string>();
                foreach (JProperty child in ((JObject)token).Properties())
                {
                    if (child.Name != "topicGuid")
                    {
                        result[child.Name] = TokenAsString(child.Value);
                    }
                }
                return result;
            }

            private static string TokenAsString(JToken token)
            {
                string stringValue;
                if (token.Type == JTokenType.String || token.Type == JTokenType.Guid)
                {
                    stringValue = token.ToObject<string>();
                }
                else
                {
                    stringValue = token.ToString(Newtonsoft.Json.Formatting.None);
                }

                return stringValue;
            }

            public void Dispose()
            {
                json?.Clear();
            }

            public bool MoveNext()
            {
                index++;
                return index < json.Count();
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}
