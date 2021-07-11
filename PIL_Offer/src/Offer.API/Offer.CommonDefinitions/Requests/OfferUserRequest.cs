using Offer.CommonDefinitions.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Offer.CommonDefinitions.Requests
{
    public class OfferUserRequest : BaseRequest
    {
        public OfferUserRecord OfferUserRecord { get; set; }
    }
}