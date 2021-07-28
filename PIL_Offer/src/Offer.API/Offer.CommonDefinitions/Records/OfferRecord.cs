using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Offer.CommonDefinitions.Records
{
    public class OfferRecord
    {
        public int Id { get; set; }
        public DateTime Creationdate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime? Validfrom { get; set; }
        public DateTime? Validto { get; set; }
        public DateTime? Modificationdate { get; set; }
        public string Discount { get; set; }
        public bool? Status { get; set; }
        public string Purpose { get; set; }
        public string Imageurl { get; set; }
        public long? Maxusagecount { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public long? LanguageId { get; set; }
        public IFormFile FormImage { get; set; }

        public bool? Valid { get; set; } // for filter
        public bool? AddUse { get; set; }// for add count

        public string ObjectTypeId { get; set; }
        public string ObjectId { get; set; }
        public long? Usedcount { get; set; }

        public string ObjectUrl { get; set; }
    }
}