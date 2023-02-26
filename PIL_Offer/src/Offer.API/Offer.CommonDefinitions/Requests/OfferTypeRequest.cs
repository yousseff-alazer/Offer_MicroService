using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Offer.CommonDefinitions.Requests
{
    public class OfferTypeRequest : BaseRequest
    {
        public OfferTypeRecord OfferTypeRecord { get; set; }
    }
}