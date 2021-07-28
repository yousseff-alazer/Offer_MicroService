using System;
using System.Collections.Generic;

#nullable disable

namespace Offer.API.Offer.DAL.DB
{
    public partial class OfferUser
    {
        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public long? Offerid { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string UserId { get; set; }
    }
}
