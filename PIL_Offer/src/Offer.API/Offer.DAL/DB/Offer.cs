using System;
using System.Collections.Generic;

#nullable disable

namespace Offer.API.Offer.DAL.DB
{
    public partial class Offer
    {
        public int Id { get; set; }
        public long? Createdby { get; set; }
        public DateTime Creationdate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Validfrom { get; set; }
        public DateTime? Validto { get; set; }
        public DateTime? Modificationdate { get; set; }
        public long? Modifiedby { get; set; }
        public string Discount { get; set; }
        public bool? Status { get; set; }
        public string Purpose { get; set; }
        public string Imageurl { get; set; }
        public long? Maxusagecount { get; set; }
        public long? Usedcount { get; set; }
        public string Languageid { get; set; }
    }
}
