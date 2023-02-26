using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Offer.CommonDefinitions.Responses
{
    public class OfferTranslateResponse : BaseResponse
    {
        [JsonProperty("Data")]
        public List<OfferTranslateRecord> OfferTranslateRecords { get; set; }
    }
}