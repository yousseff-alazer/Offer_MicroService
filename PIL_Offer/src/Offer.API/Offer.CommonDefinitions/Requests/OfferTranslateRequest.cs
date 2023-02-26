using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Offer.CommonDefinitions.Requests
{
    public class OfferTranslateRequest : BaseRequest
    {
        public OfferTranslateRecord OfferTranslateRecord { get; set; }
        public IEnumerable<OfferTranslateRecord> OfferTranslateRecords { get; set; }
    }
}