using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Offer.API.Offer.CommonDefinitions.Records
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Data
    {
        public string file;


        public string serviceName;

        public string serviceId;


        public string objectId;


        public DateTime updated_at;


        public DateTime created_at;


        public int id;
    }

    public class S3Record
    {
        [JsonPropertyName("status")]
        public bool Status;

        [JsonPropertyName("msg")]
        public string Msg;

        [JsonPropertyName("data")]
        public Data Data;
    }


}
