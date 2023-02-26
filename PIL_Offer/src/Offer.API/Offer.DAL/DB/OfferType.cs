using System;
using System.Collections.Generic;

#nullable disable

namespace Offer.API.Offer.DAL.DB
{
    public partial class OfferType
    {
        public OfferType()
        {
            Offers = new HashSet<Offer>();
        }

        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
