using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.Converters
{
    internal class AccountStateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AccountState) || objectType == typeof(TvEpisodeAccountState);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            // Sometimes the AccountState.Rated is an object with a value in it
            // In these instances, convert it from:
            //  "rated": { "value": 5 }
            //  "rated": False
            // To:
            //  "rating": 5
            //  "rating": null

            JToken obj = jObject["rated"];
            if (obj.Type == JTokenType.Boolean)
            {
                // It's "False", so the rating is not set
                jObject.Remove("rated");
                jObject.Add("rating", null);
            }
            else if (obj.Type == JTokenType.Object)
            {
                // Read out the value
                double rating = obj["value"].ToObject<double>();
                jObject.Remove("rated");
                jObject.Add("rating", rating);
            }

            object result = Activator.CreateInstance(objectType);

            // Populate the result
            serializer.Populate(jObject.CreateReader(), result);

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}