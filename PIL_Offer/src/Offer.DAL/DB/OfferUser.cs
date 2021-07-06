using System;
using System.Collections.Generic;

#nullable disable

namespace Offer.DAL.DB
{
    public partial class OfferUser
    {
        public int Id { get; set; }
        public long? Createdby { get; set; }
        public DateTime Creationdate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public long? Modifiedby { get; set; }
        public long? Offerid { get; set; }
        public long? Userid { get; set; }
    }
}
