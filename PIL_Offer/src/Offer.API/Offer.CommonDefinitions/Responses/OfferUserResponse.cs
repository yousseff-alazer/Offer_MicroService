using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Offer.CommonDefinitions.Responses
{
    public class OfferUserResponse : BaseResponse
    {
        [JsonProperty("Data")]
        public List<OfferUserRecord> OfferUserRecords { get; set; }
    }
}