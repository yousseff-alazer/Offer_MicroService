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
        public long? Createdby { get; set; }
        public DateTime Creationdate { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Modificationdate { get; set; }
        public long? Modifiedby { get; set; }
        public long? Offerid { get; set; }
        public long? Userid { get; set; }
    }
}