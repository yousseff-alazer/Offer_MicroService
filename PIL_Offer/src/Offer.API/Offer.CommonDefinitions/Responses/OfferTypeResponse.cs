using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Offer.CommonDefinitions.Responses
{
    public class OfferTypeResponse : BaseResponse
    {
        [JsonProperty("Data")]
        public List<OfferTypeRecord> OfferTypeRecords { get; set; }
    }
}