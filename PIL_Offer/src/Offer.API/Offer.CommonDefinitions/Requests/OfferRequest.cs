using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Offer.CommonDefinitions.Requests
{
    public class OfferRequest : BaseRequest
    {
        public OfferRecord OfferRecord { get; set; }
    }
}