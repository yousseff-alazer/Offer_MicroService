using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offer.CommonDefinitions.Records
{
    public class OfferUserRecord
    {
        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public long? Offerid { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string UserId { get; set; }

        public List<int> OfferIds { get; set; } //for filter
    }
}